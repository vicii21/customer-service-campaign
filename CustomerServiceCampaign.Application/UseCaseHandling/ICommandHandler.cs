using CustomerServiceCampaign.Application.Actor;
using CustomerServiceCampaign.Application.Exceptions;
using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.UseCaseHandling
{
    public interface ICommandHandler
    {
        void HandleCommand<TRequest>(ICommand<TRequest> command, TRequest data);
    }

    public class CommandHandler : ICommandHandler
    {
        private IUseCaseLogger _logger;
        private IApplicationActor _actor;
                
        public CommandHandler(IApplicationActor actor, IUseCaseLogger logger)
        {
            _actor = actor;
            _logger = logger;
        }

        public void HandleCommand<TRequest>(ICommand<TRequest> command, TRequest data)
        {
            if (_actor.AllowedUseCases.Contains(command.Id))
            {
                _logger.Add(new UseCaseLogEntry
                {
                    Actor = _actor.FullName,
                    ActorId = _actor.Id,
                    Data = data,
                    UseCaseName = command.Name,
                });
            }
            else
            {
                throw new UnauthorizedUseCaseExecutionException(_actor.FullName, command.Name);
            }
        }
    }
}
