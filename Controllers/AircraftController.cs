using Airline.DAL;
using AirlineAPI.Dataviews;
using AirlineAPI.DTO;
using AirlineAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirlineAPI.Controllers
{
    [ApiController]
    [Route("aircraft")]
    public class AircraftController : ControllerBase
    {
        [HttpPost("create")]
        public IResult Create(DAL<Aircraft> dal, [FromBody] AircraftCreateDTO createData)
        {
            dal.Register(new Aircraft(createData));

            return Results.Created();
            
        }

        [HttpGet("list")]
        public IResult List(
            DAL<Aircraft> dal, 
            [FromQuery] int page = 1, 
            [FromQuery] int perPage = 10)
        {
            return Results.Ok(dal.List().Skip((page - 1) * perPage).Take(perPage).Select(aircraft => new AircraftDetailView(aircraft)));
        }


        [HttpGet("{id}")]
        public IResult Detail(DAL<Aircraft> dal, int id)
        {
            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound(new{Message = "Aircraft not found!"});
            }

            return Results.Ok(new AircraftDetailView(aircraft));
        }

        [HttpPatch("update/{id}")]
        public IResult Update(DAL<Aircraft> dal, [FromBody] AircraftUpdateDTO updateData, int id)
        {
            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound(new{Message = "Aircraft not found!"});
            }

            aircraft.Update(updateData);
            dal.Update(aircraft);


            return Results.Ok();
        }

        [HttpDelete("{id}")]
        public IResult Remove(DAL<Aircraft> dal,int id)
        {
            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound(new{Message = "Aircraft not found!"});
            }
            dal.Remove(aircraft);

            return Results.NoContent();
        }
    }
}