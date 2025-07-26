using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Filters;

public class AuthTokenDocumentFilter : IDocumentFilter {
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
        swaggerDoc.Components.Schemas.Add("TokenResponse", new OpenApiSchema {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema> {
                ["access_token"] = new OpenApiSchema { Type = "string" },
                ["token_type"] = new OpenApiSchema { Type = "string" },
                ["expires_in"] = new OpenApiSchema { Type = "integer" },
                ["scope"] = new OpenApiSchema { Type = "string" }
            },
            Required = new HashSet<string> {"access_token", "token_type", "expires_in", "scope"}
        });

        var userBodySchema = new OpenApiSchema {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema> {
                ["grant_type"] = new OpenApiSchema { Type = "string" },
                ["scope"] = new OpenApiSchema { Type = "string" },
                ["username"] = new OpenApiSchema { Type = "string" },
                ["password"] = new OpenApiSchema { Type = "string" },
                ["client_id"] = new OpenApiSchema { Type = "string" },
                ["client_secret"] = new OpenApiSchema { Type = "string" }
            }
        };

        var tokenPath = new OpenApiPathItem {
            Operations = new Dictionary<OperationType, OpenApiOperation> {
                [OperationType.Post] = new OpenApiOperation {
                    Tags = new[] { new OpenApiTag() { Name = "JwtToken" } },
                    RequestBody = new OpenApiRequestBody {
                        Content = {
                            ["application/x-www-form-urlencoded"] = new OpenApiMediaType {
                                Schema = userBodySchema,
                            }
                        }
                    },
                    Responses = new OpenApiResponses {
                        ["200"] = new OpenApiResponse {
                            Description = "OK",
                            Content = new Dictionary<string, OpenApiMediaType> {
                                ["text/plain"] = new OpenApiMediaType {
                                    Schema = new OpenApiSchema {
                                        Reference = new OpenApiReference {
                                            Type = ReferenceType.Schema,
                                            Id = "TokenResponse"
                                        }
                                    }
                                },
                                ["application/json"] = new OpenApiMediaType {
                                    Schema = new OpenApiSchema {
                                        Reference = new OpenApiReference {
                                            Type = ReferenceType.Schema,
                                            Id = "TokenResponse"
                                        }
                                    }
                                },
                                ["text/json"] = new OpenApiMediaType {
                                    Schema = new OpenApiSchema {
                                        Reference = new OpenApiReference {
                                            Type = ReferenceType.Schema,
                                            Id = "TokenResponse"
                                        }
                                    }
                                }
                            }
                        },
                        ["400"] = new OpenApiResponse { Description = "Bad Request" }
                    }
                }
            }
        };

        swaggerDoc.Paths.Add("/connect/token", tokenPath);
    }
}
