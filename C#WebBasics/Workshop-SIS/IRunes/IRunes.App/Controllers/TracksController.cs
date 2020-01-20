namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using IRunes.Models;
    using IRunes.Services;
    using SIS.WebServer;
    using SIS.WebServer.Attributes;
    using SIS.WebServer.Result;

    public class TracksController : Controller
    {

        private ITrackService trackService;
        private IAlbumService albumService;
        public TracksController()
        {
            this.trackService = new TrackService();
            this.albumService = new AlbumService();
        }

        [Authorize]
        public ActionResult Create()
        {

            var albumId = this.Request.QueryData["albumId"].ToString();
            ViewData["AlbumId"] = albumId;

            return this.View();
        }

        [HttpPost(ActionName = "Create")]
        [Authorize]
        public ActionResult CreateConfirm()
        {

            var albumId = this.Request.QueryData["albumId"].ToString();

            string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            string link = ((ISet<string>)this.Request.FormData["link"]).FirstOrDefault();
            decimal price;

            if (!decimal.TryParse(((ISet<string>)this.Request.FormData["price"]).FirstOrDefault(), out price))
            {
                return this.Redirect($"/Tracks/Create?albumId={albumId}");
            }

            var track = new Track
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Link = link,
                Price = price,
                AlbumId = albumId
            };

            if (!this.albumService.AddTrackToAlbum(albumId, track))
            {
                return this.Redirect("Albums/All");
            }

            return this.Redirect($"/Albums/Details?albumId={albumId}");
        }

        [Authorize]
        public ActionResult Details()
        {

            var albumId = this.Request.QueryData["albumId"].ToString();
            var trackId = this.Request.QueryData["trackId"].ToString();

            var albumFromDb = this.albumService.GetAlbumById(albumId);
            var trackFromDb = this.trackService.GetTrackById(trackId);

            if (albumFromDb == null || trackFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            this.ViewData["TrackName"] = WebUtility.UrlDecode(trackFromDb.Name);
            this.ViewData["TrackLink"] = WebUtility.UrlDecode(trackFromDb.Link);
            this.ViewData["TrackPrice"] = $"${trackFromDb.Price:f2}";
            this.ViewData["AlbumId"] = albumFromDb.Id;

            return this.View();

        }
    }
}
