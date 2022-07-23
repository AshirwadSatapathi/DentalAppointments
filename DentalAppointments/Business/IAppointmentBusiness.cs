using DentalAppointments.Models;

namespace DentalAppointments.Business
{
    public interface IAppointmentBusiness
    {
         public Task<long> ScheduleAppointment(Appointment appointment);
         public Task CancelAppointment(long id);
    }
}
