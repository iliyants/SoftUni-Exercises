﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {

        private RunesDbContext context;
        public AlbumService()
        {
            this.context = new RunesDbContext();
        }

        public bool AddTrackToAlbum(string albumId, Track track)
        {
            var albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return false;
            }

            albumFromDb.Tracks.Add(track);
            albumFromDb.Price = albumFromDb
                .Tracks
                .Sum(t => t.Price) * 0.87m;

            this.context.Update(albumFromDb);
            this.context.SaveChanges();

            return true;
        }

        public Album CreateAlbum(Album album)
        {
            album = context.Albums.Add(album).Entity;
            context.SaveChanges();

            return album;
        }

        public Album GetAlbumById(string id)
        {
            return context.Albums
                .Include(album => album.Tracks)
                .SingleOrDefault(x => x.Id == id);
        }

        public ICollection<Album> GetAllAlbums()
        {
            return context.Albums.ToList();
        }
    }
}
