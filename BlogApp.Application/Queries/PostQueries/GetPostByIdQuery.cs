﻿using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.PostQueries
{
    public record GetPostByIdQuery(int Id) : IRequest<PostResponseDto>;

}
