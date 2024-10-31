using MediatR;

namespace BlogApp.Application.Commands.PostCommands
{
    public record DeletePostCommand(int PostId) : IRequest<bool>;

}
