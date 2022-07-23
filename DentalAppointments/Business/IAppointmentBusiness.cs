using DentalAppointments.Models;

namespace DentalAppointments.Business
{
    public interface IAppointmentBusiness
    {
         public Task<long> ScheduleAppointmentNotification(Appointment appointment);
         public Task CancelAppointmentNotification(long id);
    }
}
