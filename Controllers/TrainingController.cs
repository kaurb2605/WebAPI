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
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class TrainingController :ApiController 
    {
        private DataModelClassesDataContext  db = new DataModelClassesDataContext();

        // GET: api/Item
        public IQueryable<Training> Get()
        {
            return db.Trainings;
        }

        // GET api/<controller>/5
        public HttpResponseMessage GetByID(int id)
        {

            var traininginfo = (from t in db.Trainings
                                where t.Id == id
                                select t).FirstOrDefault();
            if (traininginfo != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, traininginfo);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Training details not found");
            }

        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Training _training)
        {
            try
            {
                db.Trainings.InsertOnSubmit(_training);
                db.SubmitChanges();

                var dayscount = (_training.StartDate - _training.EndDate).Days;

                var message = Request.CreateResponse(HttpStatusCode.Created, _training);

                message.Headers.Location = new Uri(Request.RequestUri + _training.Id.ToString() + " Total Days of training = " + dayscount.ToString());

                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Update(int id, [FromBody]Training _training)
        {
            var traininginfo = (from t in db.Trainings
                                where t.Id == id
                                select t).FirstOrDefault();
            if (traininginfo != null)
            {
                traininginfo.Name = _training.Name;
                traininginfo.StartDate = _training.StartDate;
                traininginfo.EndDate = _training.EndDate;

                db.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, traininginfo);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Training details not found");
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var traininginfo = (from t in db.Trainings
                                    where t.Id == id
                                    select t).FirstOrDefault();
                if (traininginfo != null)
                {


                    db.Trainings.DeleteOnSubmit(traininginfo);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Training details not found for " + id.ToString());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}