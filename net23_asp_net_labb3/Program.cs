using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb3.Data;
using net23_asp_net_labb3.Models;
using net23_asp_net_labb3.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


// Hämta alla personer i systemet
app.MapGet("/people", async (ApplicationDbContext context) =>
{
    var people = await context.People
    .Include(p => p.PeopleInterests)
    .ThenInclude(pi => pi.Interest)
    .ThenInclude(i => i.Links)
    .ToListAsync();
    if (people == null || people.Count == 0)
    {
        return Results.NotFound("No people found in the database");
    }

    var peopleDTOs = people.Select(p => new PeopleDTO
    {
        Id = p.Id,
        Name = p.Name,
        Age = p.Age,
        PhoneNumber = p.PhoneNumber,
        Interests = p.PeopleInterests.Select(pi => new InterestDTO
        {
            Id = pi.Interest.Id,
            Title = pi.Interest.Title,
            Description = pi.Interest.Description,
            Links = pi.Interest.Links.Select(l => new LinkDTO
            {
                Id = l.Id,
                Website = l.Website
            }).ToList()
        }).ToList()
    }).ToList();

    return Results.Ok(peopleDTOs);
});


// Hämta alla intressen som är kopplade till en specifik person
app.MapGet("/people/{id:int}", async (int id, ApplicationDbContext context) =>
{
    var peopleInterests = await context.PeopleInterests
    .Where(pi => pi.PeopleId == id)
    .Include(pi => pi.Interest)
    .ThenInclude(i => i.Links)
    .Select(pi => new InterestDTO
    {
        Id = pi.InterestId,
        Title = pi.Interest.Title,
        Description = pi.Interest.Description,
        Links = pi.Interest.Links.Select(l => new LinkDTO
        {
            Id = l.Id,
            Website = l.Website
        }).ToList()
    }).ToListAsync();


    if (peopleInterests == null || peopleInterests.Count == 0)
    {
        return Results.NotFound("No person found in the database with the specific Id");
    }
    return Results.Ok(peopleInterests);
});


// Hämta alla länkar som är kopplade till en specifik person
app.MapGet("/people/{id:int}/links", async (int id, ApplicationDbContext context) =>
{
    var peopleLinks = await context.PeopleInterests
    .Where(pi => pi.PeopleId == id)
    .Include(pi => pi.Interest)
    .ThenInclude(i => i.Links)
    .SelectMany(pi => pi.Interest.Links)
    .Select(link => new LinkDTO
    {
        Id = link.Id,
        Website = link.Website
    }).ToListAsync();

    if (peopleLinks == null || peopleLinks.Count == 0)
    {
        return Results.NotFound("No person found in the database with the specific Id");
    }
    return Results.Ok(peopleLinks);
});


// Koppla en person till ett nytt intresse
app.MapPost("/people/{personId:int}/interests", async (int personId, InterestDTO interestDTO, ApplicationDbContext context) =>
{
    var person = await context.People.FindAsync(personId);
    if (person == null)
    {
        return Results.NotFound("Person with the specific ID was not found.");
    }

    var interest = new Interest
    {
        Title = interestDTO.Title,
        Description = interestDTO.Description
    };
    context.Add(interest);
    await context.SaveChangesAsync();

    var newPeopleInterest = new PeopleInterest
    {
        PeopleId = personId,
        InterestId = interest.Id
    };
    context.PeopleInterests.Add(newPeopleInterest);

    await context.SaveChangesAsync();

    return Results.Created($"/people/{personId}/interests", null);
});
//app.MapPost("/people/{personId:int}/{title}/{description}", async (int personId, string title, string description, ApplicationDbContext context) =>
//{
//    var person = await context.People.FindAsync(personId);
//    if (person == null)
//    {
//        return Results.NotFound("Person with the specific ID was not found.");
//    }

//    var interest = new Interest
//    {
//        Title = title,
//        Description = description
//    };
//    context.Add(interest);
//    await context.SaveChangesAsync();

//    var newPeopleInterest = new PeopleInterest
//    {
//        PeopleId = personId,
//        InterestId = interest.Id
//    };
//    context.PeopleInterests.Add(newPeopleInterest);

//    await context.SaveChangesAsync();

//    return Results.Created($"/people/{personId}/{title}/{description}", null);
//});


// Lägga in nya länkar för en specifik person och ett specifikt intresse
app.MapPost("/people/{personId:int}/interests/{interestId:int}/link", async (int personId, int interestId, LinkDTO linkDto, ApplicationDbContext context) =>
{
    var person = await context.People.FindAsync(personId);
    if (person == null)
    {
        return Results.NotFound("Person with the specific ID was not found.");
    }

    var interest = await context.PeopleInterests
        .Where(pi => pi.PeopleId == personId && pi.InterestId == interestId)
        .Include(pi => pi.Interest)
        .Select(pi => pi.Interest)
        .FirstOrDefaultAsync();

    if (interest == null)
    {
        return Results.NotFound("Person with the specific ID does not have any interests.");
    }

    var link = new Link
    {
        Website = linkDto.Website,
        InterestId = interestId
    };

    context.Links.Add(link);
    await context.SaveChangesAsync();

    interest.Links.Add(link);

    await context.SaveChangesAsync();

    return Results.Created($"/people/{personId}/interests/{interestId}/link", null);
});

// FLERA länkar
app.MapPost("/people/{personId:int}/interests/{interestId:int}/links", async (int personId, int interestId, List<LinkDTO> linkDtos, ApplicationDbContext context) =>
{
    var person = await context.People.FindAsync(personId);
    if (person == null)
    {
        return Results.NotFound("Person with the specific ID was not found.");
    }

    var interest = await context.PeopleInterests
        .Where(pi => pi.PeopleId == personId && pi.InterestId == interestId)
        .Include(pi => pi.Interest)
        .Select(pi => pi.Interest)
        .FirstOrDefaultAsync();

    if (interest == null)
    {
        return Results.NotFound("Person with the specific ID does not have any interests.");
    }
    var links = new List<Link>();

    foreach (var linkDto in linkDtos)
    {
        var link = new Link
        {
            Website = linkDto.Website,
            InterestId = interestId
        };

        context.Links.Add(link);

        links.Add(link);
    }

    await context.SaveChangesAsync();

    foreach (var link in links)
    {
        interest.Links.Add(link);
    }

    await context.SaveChangesAsync();

    return Results.Created($"/people/{personId}/interests/{interestId}/links", null);
});

app.Run();


