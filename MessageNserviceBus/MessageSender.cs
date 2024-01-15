using Framework.Contracts;

namespace MessageNServiceBus
{
    public class MessageSender : IMessageService
    {
        private readonly IMessageSession message;

        public MessageSender(IMessageSession message)
        {
            this.message = message;
        }

        public async Task SendMessageAsync(object obj)
        {
            await message.Send(obj).ConfigureAwait(false);
        }

        public async Task PublishMessageAsync(object obj)
        {
            await message.Publish(obj).ConfigureAwait(false);
        }
    }
}