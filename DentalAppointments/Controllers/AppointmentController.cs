using DentalAppointments.Business;
using DentalAppointments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalAppointments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public IAppointmentBusiness _appointmentBusiness { get; set; }
        public AppointmentController(IAppointmentBusiness appointmentBusiness)
        {
            _appointmentBusiness = appointmentBusiness;
        }


        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduledAppointmentInQueue([FromBody] Appointment appointment)
        {
            try
            {
                long msgSequenceNumber = await _appointmentBusiness.ScheduleAppointment(appointment);

                return new OkObjectResult(new { MessageSequenceNumber = msgSequenceNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{messageSequenceNumber:long}")]
        public async Task<IActionResult> CancelAppointmentFromQueue([FromRoute] int messageSequenceNumber)
        {
            try
            {
                if (messageSequenceNumber < 0)
                {
                    return new BadRequestObjectResult("Invalid value of messageSequenceNumber");
                }
                await _appointmentBusiness.CancelAppointment(messageSequenceNumber);
                return new OkObjectResult("Scheduled message has been discarded.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
