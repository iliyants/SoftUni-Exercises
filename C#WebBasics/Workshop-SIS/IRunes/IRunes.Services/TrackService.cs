using System;
using System.Linq;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        private readonly RunesDbContext context;
        public TrackService()
        {
            this.context = new RunesDbContext();
        }

        public Track GetTrackById(string id)
        {
            return this.context.Tracks
                .SingleOrDefault(x => x.Id == id);
        }
    }
}
