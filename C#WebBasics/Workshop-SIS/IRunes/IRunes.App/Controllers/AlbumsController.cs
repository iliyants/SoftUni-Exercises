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
    using SIS.WebServer.Mapper;
    using IRunes.App.ViewModels;

    public class AlbumsController : Controller
    {

        private readonly IAlbumService albumService;

        public AlbumsController()
        {
            this.albumService = new AlbumService();
        }

        [Authorize]
        public ActionResult All()
        {

            if (!albumService.GetAllAlbums().Any())
            {
                this.ViewData["Albums"] = "There are currently no albums.";
            }

            else
            {
                this.ViewData["Albums"] =
                string.Join("<br/>",
                albumService.GetAllAlbums()
                .Select(a => $"<a class=\"text-primary font-weight-bold\" href=/Albums/Details?albumId={a.Id}>{WebUtility.UrlDecode(a.Name)}</a>")
                .ToList());
            }

            return this.View();

        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Create")]
        [Authorize]
        public ActionResult CreateConfirm()
        {

            var name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            var cover = ((ISet<string>)this.Request.FormData["cover"]).FirstOrDefault();

            var album = new Album
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Cover = cover,
                Price = 0m
            };

            this.albumService.CreateAlbum(album);

            return this.Redirect("/Albums/All");

        }

        [Authorize]
        public ActionResult Details()
        {

            var albumId = this.Request.QueryData["albumId"].ToString();

            var albumFromDb = this.albumService.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            this.ViewData["AlbumId"] = albumFromDb.Id;
            this.ViewData["AlbumName"] = WebUtility.UrlDecode(albumFromDb.Name);
            this.ViewData["AlbumCover"] = WebUtility.UrlDecode(albumFromDb.Cover);
            this.ViewData["AlbumPrice"] = $"${albumFromDb.Price:f2}";

            var tracks = albumFromDb.Tracks.ToList();
            var tracksHtml = string.Empty;

            if (!tracks.Any())
            {
                tracksHtml = "<p>Nothing to show...</p>" +
                             Environment.NewLine +
                             "<p>This album has no tracks added yet!</p>";
            }

            else
            {
                for (int i = 0; i < tracks.Count; i++)
                {
                    tracksHtml += $"<li>{i + 1}. <a class=\"text-primary font-weight-bold\" href=\"/Tracks/Details?albumId={tracks[i].AlbumId}&trackId={tracks[i].Id}\">" + WebUtility.UrlDecode(tracks[i].Name) + "</a></li>";
                }
            }

            this.ViewData["AlbumTracks"] = tracksHtml;

            return this.View();
        }
    }
}
