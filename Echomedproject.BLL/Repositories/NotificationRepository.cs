﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Repositories
{
    public class NotificationRepository: GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(EckomedDbContext dbcontext) : base(dbcontext) { }

    }
}
