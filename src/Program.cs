using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//Start service discovery at /.well-known/terraform.json
app.MapGet(".well-known/terraform.json", (string @namespace, string name, string system, string version) => 
{
    //temporarily implement constant values.
    return Results.Json(@"{
        ""modules.v1"": ""/v1/modules"",
        ""providers.v1"": ""/v1/providers"",
    }");
});

//Start module registry system.
app.MapGet("/v1/modules/{namespace}/{name}/{system}", (string @namespace, string name, string system) => 
{
    var json = $@"{{
        ""namespace"": ""{@namespace}"",
        ""name"": ""{name}"",
        ""system"": ""{system}"",
    }}";
    //temporarily implement constant values.
    return Results.Json(json);
});
app.MapGet("/v1/modules/{namespace}/{name}/{system}/versions", (string @namespace, string name, string system) => 
{
    var json = @"{
    ""modules"": [
        {
            ""versions"": [
                {""version"": ""1.0.0""},
                {""version"": ""1.1.0""},
                {""version"": ""2.0.0""}
            ]
        }
    ]}";
    //temporarily implement constant values.
    return Results.Json(json);
});
app.MapGet("/v1/modules/{namespace}/{name}/{system}/{version}/download", (string @namespace, string name, string system, string version, HttpContext httpContext) => 
{
    //temporarily implement constant values.
    httpContext.Response.Headers.Add("X-Terraform-Get", "https://api.github.com/repos/hashicorp/terraform-aws-consul/tarball/v0.0.1//*?archive=tar.gz");
    return Results.NoContent;
});

//start provider api
app.MapGet("/v1/providers/{namespace}/{type}/versions", (string @namespace, string type) => 
{
    var json = @"{
    ""versions"": [
        {
        ""version"": ""2.0.0"",
        ""protocols"": [""4.0"", ""5.1""],
        ""platforms"": [
            {""os"": ""darwin"", ""arch"": ""amd64""},
            {""os"": ""linux"", ""arch"": ""amd64""},
            {""os"": ""linux"", ""arch"": ""arm""},
            {""os"": ""windows"", ""arch"": ""amd64""}
        ]
        },
        {
        ""version"": ""2.0.1"",
        ""protocols"": [""5.2""],
        ""platforms"": [
            {""os"": ""darwin"", ""arch"": ""amd64""},
            {""os"": ""linux"", ""arch"": ""amd64""},
            {""os"": ""linux"", ""arch"": ""arm""},
            {""os"": ""windows"", ""arch"": ""amd64""}
        ]
        }
    ]
    }";
    //temporarily implement constant values.
    return Results.Json(json);
});
//:namespace/:type/:version/download/:os/:arch
app.MapGet("/v1/providers/{namespace}/{type}/{version}/download/{os}/{arch}", (string @namespace, string type, string version, string os, string arch) => 
{
    var json = @"{
        ""protocols"": [""4.0"", ""5.1""],
        ""os"": ""linux"",
        ""arch"": ""amd64"",
        ""filename"": ""terraform-provider-random_2.0.0_linux_amd64.zip"",
        ""download_url"": ""https://releases.hashicorp.com/terraform-provider-random/2.0.0/terraform-provider-random_2.0.0_linux_amd64.zip"",
        ""shasums_url"": ""https://releases.hashicorp.com/terraform-provider-random/2.0.0/terraform-provider-random_2.0.0_SHA256SUMS"",
        ""shasums_signature_url"": ""https://releases.hashicorp.com/terraform-provider-random/2.0.0/terraform-provider-random_2.0.0_SHA256SUMS.sig"",
        ""shasum"": ""5f9c7aa76b7c34d722fc9123208e26b22d60440cb47150dd04733b9b94f4541a"",
        ""signing_keys"": {
        ""gpg_public_keys"": [
            {
            ""key_id"": ""51852D87348FFC4C"",
            ""ascii_armor"": ""-----BEGIN PGP PUBLIC KEY BLOCK-----\nVersion: GnuPG v1\n\nmQENBFMORM0BCADBRyKO1MhCirazOSVwcfTr1xUxjPvfxD3hjUwHtjsOy/bT6p9f\nW2mRPfwnq2JB5As+paL3UGDsSRDnK9KAxQb0NNF4+eVhr/EJ18s3wwXXDMjpIifq\nfIm2WyH3G+aRLTLPIpscUNKDyxFOUbsmgXAmJ46Re1fn8uKxKRHbfa39aeuEYWFA\n3drdL1WoUngvED7f+RnKBK2G6ZEpO+LDovQk19xGjiMTtPJrjMjZJ3QXqPvx5wca\nKSZLr4lMTuoTI/ZXyZy5bD4tShiZz6KcyX27cD70q2iRcEZ0poLKHyEIDAi3TM5k\nSwbbWBFd5RNPOR0qzrb/0p9ksKK48IIfH2FvABEBAAG0K0hhc2hpQ29ycCBTZWN1\ncml0eSA8c2VjdXJpdHlAaGFzaGljb3JwLmNvbT6JATgEEwECACIFAlMORM0CGwMG\nCwkIBwMCBhUIAgkKCwQWAgMBAh4BAheAAAoJEFGFLYc0j/xMyWIIAIPhcVqiQ59n\nJc07gjUX0SWBJAxEG1lKxfzS4Xp+57h2xxTpdotGQ1fZwsihaIqow337YHQI3q0i\nSqV534Ms+j/tU7X8sq11xFJIeEVG8PASRCwmryUwghFKPlHETQ8jJ+Y8+1asRydi\npsP3B/5Mjhqv/uOK+Vy3zAyIpyDOMtIpOVfjSpCplVRdtSTFWBu9Em7j5I2HMn1w\nsJZnJgXKpybpibGiiTtmnFLOwibmprSu04rsnP4ncdC2XRD4wIjoyA+4PKgX3sCO\nklEzKryWYBmLkJOMDdo52LttP3279s7XrkLEE7ia0fXa2c12EQ0f0DQ1tGUvyVEW\nWmJVccm5bq25AQ0EUw5EzQEIANaPUY04/g7AmYkOMjaCZ6iTp9hB5Rsj/4ee/ln9\nwArzRO9+3eejLWh53FoN1rO+su7tiXJA5YAzVy6tuolrqjM8DBztPxdLBbEi4V+j\n2tK0dATdBQBHEh3OJApO2UBtcjaZBT31zrG9K55D+CrcgIVEHAKY8Cb4kLBkb5wM\nskn+DrASKU0BNIV1qRsxfiUdQHZfSqtp004nrql1lbFMLFEuiY8FZrkkQ9qduixo\nmTT6f34/oiY+Jam3zCK7RDN/OjuWheIPGj/Qbx9JuNiwgX6yRj7OE1tjUx6d8g9y\n0H1fmLJbb3WZZbuuGFnK6qrE3bGeY8+AWaJAZ37wpWh1p0cAEQEAAYkBHwQYAQIA\nCQUCUw5EzQIbDAAKCRBRhS2HNI/8TJntCAClU7TOO/X053eKF1jqNW4A1qpxctVc\nz8eTcY8Om5O4f6a/rfxfNFKn9Qyja/OG1xWNobETy7MiMXYjaa8uUx5iFy6kMVaP\n0BXJ59NLZjMARGw6lVTYDTIvzqqqwLxgliSDfSnqUhubGwvykANPO+93BBx89MRG\nunNoYGXtPlhNFrAsB1VR8+EyKLv2HQtGCPSFBhrjuzH3gxGibNDDdFQLxxuJWepJ\nEK1UbTS4ms0NgZ2Uknqn1WRU1Ki7rE4sTy68iZtWpKQXZEJa0IGnuI2sSINGcXCJ\noEIgXTMyCILo34Fa/C6VCm2WBgz9zZO8/rHIiQm1J5zqz0DrDwKBUM9C\n=LYpS\n-----END PGP PUBLIC KEY BLOCK-----"",
            ""trust_signature"": """",
            ""source"": ""HashiCorp"",
            ""source_url"": ""https://www.hashicorp.com/security.html""
            }
        ]
        }
    }";
    //temporarily implement constant values.
    return Results.Json(json);
});

app.Run();