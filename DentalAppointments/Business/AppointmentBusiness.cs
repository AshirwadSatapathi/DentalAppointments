using Azure.Messaging.ServiceBus;
using DentalAppointments.Models;
using Newtonsoft.Json;

namespace DentalAppointments.Business
{
    public class AppointmentBusiness : IAppointmentBusiness
    {
        public IConfiguration _configuration { get; set; }
        public ServiceBusClient _serviceBusClient { get; set; }
        public ServiceBusSender _serviceBusSender { get; set; }

        public AppointmentBusiness(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceBusClient = serviceBusClient;
            _serviceBusSender = _serviceBusClient.CreateSender(_configuration.GetValue<String>("queueName"));

        }
        public async Task<long> ScheduleAppointment(Appointment appointment)
        {
            //Serialize the appointment object
            String serializedContent = JsonConvert.SerializeObject(appointment);
            //Create a service bus message which contains the serialized appointment data  
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(serializedContent);
            //Schedules the message in the service bus queue 
            var messageSequenceNumber = await _serviceBusSender
                .ScheduleMessageAsync(serviceBusMessage, appointment.ScheduledAt);
            //Returns the message sequence number of the scheduled message in service bus queue
            return messageSequenceNumber;
        }

        public async Task CancelAppointment(long messageSequenceNumber)
        {
            await _serviceBusSender.CancelScheduledMessageAsync(messageSequenceNumber);
        }
    }
}
