﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LoanMe.ApplicationBlocks.EventBus.CommandBus
{
    public interface IIntegrationCommandHandler
    {
        void Handle(IntegrationCommand command);
    }

    public interface IIntegrationCommandHandler<T> : IIntegrationCommandHandler
    {
        void Handle(IntegrationCommand<T> command);
    }
}
