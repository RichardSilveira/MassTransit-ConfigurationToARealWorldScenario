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
                        .SelectId(context => context.Message.CorrelationId));

            Event(() => ApproveOrder, cc => cc.CorrelateById(context => context.Message.CorrelationId));
            Event(() => RejectOrder, cc => cc.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(RegisterOrder)
                    .Then(context =>
                    {
                        context.Instance.OrderID = context.Data.OrderID;//Check if already is fullfield...
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
                        context.Instance.OrderID = context.Data.OrderID;//Check if already is fullfield...
                        context.Instance.EstimatedTime = context.Data.EstimatedTime;
                        context.Instance.Status = context.Data.Status;
                    })
                    .ThenAsync(async context => await Console.Out.WriteLineAsync(string.Format("Send notification to client {0} with phone numer: {1} about your order approved.",
                                                                                                context.Instance.CustomerName, context.Instance.CustomerPhone)))
                    .TransitionTo(Approved),
                //.Publish(context => new OrderApprovedEvent(context.Instance))//In this scenario, i don´t need of this event...
                When(RejectOrder)
                    .Then(context =>
                    {
                        context.Instance.OrderID = context.Data.OrderID;//Check if already is fullfield...
                        context.Instance.RejectedReasonPhrase = context.Data.RejectedReasonPhrase;
                    })
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }

        public State Registered { get; private set; }
        public State Approved { get; private set; }
        public State Closed { get; private set; }

        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }
        public Event<IApproveOrderCommand> ApproveOrder { get; private set; }
        public Event<ICloseOrderCommand> CloseOrder { get; private set; }
        public Event<IRejectOrderCommand> RejectOrder { get; private set; }

    }
}
