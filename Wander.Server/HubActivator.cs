﻿using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server
{
    public class HubActivator : IHubActivator
    {
        private readonly IKernel container;

        public HubActivator(IKernel container)
        {
            this.container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub) container.GetService(descriptor.HubType);
        }
    }
}