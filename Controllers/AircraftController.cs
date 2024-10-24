using Airline.DAL;
using Airline.Database;
using AirlineAPI.Dataviews;
using AirlineAPI.DTO;
using AirlineAPIV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirlineAPI.Controllers
{
    [ApiController]
    [Route("aircraft")]
    public class AircraftController : ControllerBase
    {
        [HttpPost("create")]
        public IResult Create([FromBody] AircraftCreateDTO createData)
        {
            var context = new AirlineContext();
            var dal = new DAL<Aircraft>(context);
            dal.Register(new Aircraft(createData));

            return Results.Created();
            
        }

        [HttpGet("list")]
        public IResult List()
        {
            var context = new AirlineContext();
            var dal = new DAL<Aircraft>(context);

            return Results.Ok(dal.List());
        }


        [HttpGet("{id}")]
        public IResult Detail(int id)
        {
            var context = new AirlineContext();
            var dal = new DAL<Aircraft>(context);

            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound("Aircraft not found!");
            }

            return Results.Ok(new AircraftDetailView(aircraft));
        }

        [HttpPost("update/{id}")]
        public IResult Update([FromBody] AircraftUpdateDto updateData, int id)
        {
            var context = new AirlineContext();
            var dal = new DAL<Aircraft>(context);

            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound("Aircraft not found!");
            }

            aircraft.Update(updateData);
            dal.Update(aircraft);


            return Results.Ok();
        }

        [HttpDelete("{id}")]
        public IResult Remove(int id)
        {
            var context = new AirlineContext();
            var dal = new DAL<Aircraft>(context);
    
            var aircraft = dal.GetById(id);

            if(aircraft == null)
            {
                return Results.NotFound("Aircraft not found!");
            }
            dal.Remove(aircraft);

            return Results.NoContent();
        }
    }


}