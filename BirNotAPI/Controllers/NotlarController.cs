using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BirNotAPI.DTOs;
using BirNotAPI.Extensions;
using BirNotAPI.Models;
using Microsoft.AspNet.Identity;

namespace BirNotAPI.Controllers
{
    [Authorize]
    public class NotlarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string UserId => User.Identity.GetUserId(); //UserId'nin sadece get metodu var
                                                           // public string UserId { get { return User.Identity.GetUserId(); } }


        // GET: api/Notlar
        public IQueryable<GetNotDto> GetNots()
        {
            return db.Notlar
                 .Where(x => x.YazarId == UserId)
                 .Select(x => new GetNotDto()
                 {
                     Id = x.Id,
                     Baslik = x.Baslik,
                     Icerik = x.Icerik,
                     SonDegistirilme = x.SonDegistirilme
                 });
        }

        // GET: api/Notlar/5
        [ResponseType(typeof(Not))]
        public IHttpActionResult GetNot(int id)
        {
            Not not = db.Notlar.Find(id);
            if (not == null || not.YazarId != UserId)
            {
                return NotFound();
            }

            return Ok(not.ToGetNotDTO());
        }

        // PUT: api/Notlar/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNot(int id, PutNotDto notDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notDto.Id)
            {
                return BadRequest();
            }

            Not not = db.Notlar.Find(id);
            if (not == null || not.YazarId != UserId)
            {
                return NotFound();
            }

            not.Baslik = notDto.Baslik;
            not.Icerik = notDto.Icerik;
            not.SonDegistirilme = DateTime.Now;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notlar
        [ResponseType(typeof(GetNotDto))]
        public IHttpActionResult PostNot(YeniNotDto notDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Not not = new Not() { Baslik = notDto.Baslik, Icerik = notDto.Icerik, YazarId = UserId };
            db.Notlar.Add(not);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = not.Id }, not.ToGetNotDTO());
        }

        // DELETE: api/Notlar/5
        [ResponseType(typeof(GetNotDto))]
        public IHttpActionResult DeleteNot(int id)
        {
            Not not = db.Notlar.Find(id);
            if (not == null || not.YazarId != UserId)
            {
                return NotFound();
            }

            db.Notlar.Remove(not);
            db.SaveChanges();

            return Ok(not.ToGetNotDTO());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotExists(int id)
        {
            return db.Notlar.Count(e => e.Id == id) > 0;
        }
    }
}