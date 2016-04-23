using Automatonymous;
using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<Order>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => RegisterOrder,
                cc => cc.CorrelateBy(order => order.CorrelationId,
                                    context => context.Message.EventID)
                        .SelectId(context => context.Message.EventID.Value));

            Event(() => OrderApproved, cc => cc.CorrelateById(context => context.Message.EventID.Value));

            Initially(
                When(RegisterOrder)
                    .Then(context =>
                    {
                        context.Instance.OrderID = context.Data.OrderID;
                        context.Instance.CustomerName = context.Data.CustomerName;
                        context.Instance.CustomerPhone = context.Data.CustomerPhone;
                        context.Instance.PizzaID = context.Data.PizzaID;
                    })
                    .TransitionTo(Registered)
                    .Publish(context => new OrderRegisteredEvent(context.Instance))
                );

            During(Registered,
                When(OrderApproved)
                    .Then(context =>
                    {
                        context.Instance.EstimatedTime = context.Data.EstimatedTime;
                        context.Instance.Status = context.Data.Status;
                    })
                    .Finalize()
                );
        }

        public State Registered { get; private set; }
        public State Approved { get; private set; }
        public State Rejected { get; private set; }
        public State Closed { get; private set; }

        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }
        public Event<IOrderApprovedEvent> OrderApproved { get; private set; }
    }
}
