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
            Event(() => CloseOrder, cc => cc.CorrelateById(context => context.Message.CorrelationId));
            Event(() => RejectOrder, cc => cc.CorrelateById(context => context.Message.CorrelationId));

            //TODO:notify the attendant about an approved order that is taking too long to be completed
            Schedule(() => OrderMaxTimeExpired, ce => ce.ExpirationId, sc =>
            {
                sc.Delay = TimeSpan.FromSeconds(10);//Simulated time (on real case this value will be one hour)
                sc.Received = a => a.CorrelateById(context => context.Message.CorrelationId);
            });

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
            //.ThenAsync(async context => await Console.Out.WriteLineAsync(string.Format("Send notification to client {0} with phone numer: {1} about your order status 'APPROVED'.",
            //                                                                                    context.Instance.CustomerName, context.Instance.CustomerPhone)))
            During(Registered,
                When(ApproveOrder)
                    .Then(context =>
                    {
                        context.Instance.EstimatedTime = context.Data.EstimatedTime;
                        context.Instance.Status = context.Data.Status;
                    })
                    .TransitionTo(Approved)
                    .Schedule(OrderMaxTimeExpired, context => new OrderMaxTimeExpiredEvent(context.Instance)),
                //.Publish(context => new OrderApprovedEvent(context.Instance))//In this scenario, i don´t need of this event...
                When(RejectOrder)
                    .Then(context =>
                    {
                        context.Instance.RejectedReasonPhrase = context.Data.RejectedReasonPhrase;
                    })
                    .ThenAsync(async context => await Console.Out.WriteLineAsync(string.Format("Send notification to client {0} with phone numer {1} about your order status 'REJECTED', reason: {2}.",
                                                                                                context.Instance.CustomerName, context.Instance.CustomerPhone, context.Instance.RejectedReasonPhrase)))
                    .Finalize()
                );

            During(Approved,
                When(OrderMaxTimeExpired.Received)
                    .ThenAsync(async context => await Console.Out.WriteLineAsync(string.Format("Send notification to attendant to inform about an order that is taking too long to get ready (orderID: {0}, estimated time: {1}, customerName: {2}.",
                                                                                                context.Instance.OrderID, context.Instance.EstimatedTime, context.Instance.CustomerName))),
                When(CloseOrder)
                    .Then(context => context.Instance.Status = context.Data.Status)
                    .ThenAsync(async context => await Console.Out.WriteLineAsync(string.Format("Send notification to client {0} with phone numer: {1} about your order status 'CLOSED'",
                                                                                                context.Instance.CustomerName, context.Instance.CustomerPhone)))
                    //.Unschedule(OrderMaxTimeExpired)
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }

        public State Registered { get; private set; }
        public State Approved { get; private set; }
        //Should add Closed state?
        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }
        public Event<IApproveOrderCommand> ApproveOrder { get; private set; }
        public Event<ICloseOrderCommand> CloseOrder { get; private set; }
        public Event<IRejectOrderCommand> RejectOrder { get; private set; }
        public Schedule<Order, IOrderMaxTimeExpiredEvent> OrderMaxTimeExpired { get; set; }

    }
}
