using Microsoft.AspNetCore.Mvc;

namespace Service.Api
{
    public class ConnectionTestController: ApiControllerBase
    {

        [HttpGet("TestConnection")]
        public string TestConnection() => "Hello from ConnectionTestController";
        
    }
}
