using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLC.WebApp.Models;
using PLC.WebApp.Models.Dtos;
using PLC.WebApp.Services;

namespace PLC.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlcController : ControllerBase
    {
        private SLMPConnection _slmpConnection;

        public CancellationTokenSource cancellationTokenSource;
        public CancellationToken cancellationToken;

        public PlcController(SLMPConnection slmpConnection)
        {
            _slmpConnection = slmpConnection;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectToTcp([FromBody] IpAddress ipAddress)
        {
            if (ipAddress == null)
            {
                return BadRequest();
            }

            try
            {
                _slmpConnection.CreateSlmpConnection(ipAddress);

                return Ok(new { Message = "Connected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Ip address and port not found" });

            }

        }

        [HttpPost("disconnect")]
        public async Task<IActionResult> Disconnect()
        {
            try
            {
                _slmpConnection.Disconnect();
                return Ok(new { Message = "Disconnected" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to disconnect {ex.Message}");
            }
        }


        [HttpPost("plcTaskStart")]
        public async Task<IActionResult> OnPlcTaskStart()
        {
            if (!_slmpConnection.IsConnected())
                return BadRequest(new { Message = "Plc Connection has been disconnected." });

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            try
            {
                _slmpConnection.OpenTask(cancellationTokenSource, cancellationToken);
                return Ok(new { Message = "Hub Connection OK" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Plc Connection Task Canot start." + ex.Message });
            }
        }

        [HttpPost("counterReset")]
        public async Task<IActionResult> OnCounterReset()
        {
            if (!_slmpConnection.IsConnected())
                return BadRequest(new { Message = "Plc Connection has been disconnected." });

            try
            {
                _slmpConnection.CounterReset();
                return Ok(new { Message = "Counter reset is ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("xwrite")]
        public async Task<IActionResult> xWrite([FromBody] xDto xDto)
        {
            if (!_slmpConnection.IsConnected())
                return BadRequest(new { Message = "Plc Connection has been disconnected." });

            try
            {
                _slmpConnection.Xwrite(xDto);
                //Console.WriteLine(xDto.BooleanData[0]);
                return Ok(new { Message = "X write Ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("dwrite")]
        public async Task<IActionResult> dWrite([FromBody] dDto dDto)
        {
            if (!_slmpConnection.IsConnected())
                return BadRequest(new { Message = "Plc Connection has been disconnected." });

            try
            {
                _slmpConnection.Dwrite(dDto);
                //Console.WriteLine(xDto.BooleanData[0]);
                return Ok(new { Message = "X write Ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
