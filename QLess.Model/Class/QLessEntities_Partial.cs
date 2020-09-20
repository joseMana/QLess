using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;


namespace QLess.Model
{
    public partial class QLessEntities
    {
        private int _transportCardId;
        private readonly DbChangeTracker _tracker;

        public QLessEntities(int transportCard)
            : base("name=QLessEntities")
        {
            _transportCardId = transportCard;
            _tracker = ChangeTracker;
            _tracker.DetectChanges();
        }
        public static QLessEntities Create()
        {
            return new QLessEntities();
        }
    }
}
