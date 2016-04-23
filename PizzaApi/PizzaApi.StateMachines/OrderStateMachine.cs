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
                cc => cc.CorrelateBy(order => order.OrderID,
                                    context => context.Message.OrderID)
                        .SelectId(context => Guid.NewGuid()));

            Event(() => ApproveOrder, cc => cc.CorrelateById(context => context.Message.CorrelationId));

            //Event(() => OrderApproved, cc => cc.CorrelateById(context => context.Message.EventID.Value));//Should be published only?

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
                When(ApproveOrder)
                    .Then(context =>
                    {
                        context.Instance.OrderID = context.Data.OrderID;
                        context.Instance.EstimatedTime = context.Data.EstimatedTime;
                        context.Instance.Status = context.Data.Status;
                    })
                    .ThenAsync(async context => await Console.Out.WriteLineAsync("Send notification to client about your order approved"))
                    .Finalize()
                );
        }

        public State Registered { get; private set; }
        public State Approved { get; private set; }
        public State Rejected { get; private set; }
        public State Closed { get; private set; }

        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }
        public Event<IApproveOrderCommand> ApproveOrder { get; private set; }
        //public Event<IOrderApprovedEvent> OrderApproved { get; private set; }
    }
}
