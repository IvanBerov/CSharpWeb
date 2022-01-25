# CSharpWeb

A simple C# web server created for educational purposes.

Steps to create a similar web server from scratch:

    Choose the localhost IP address (127.0.0.1) and a free local port
    Create a TcpListener and accept incomming client request asynchronously
    Write a valid HTTP response and convert it to a byte array
    Add Content-Type and Content-Length headers (be careful with UTF8 characters)
    Read the request in chunks (1024 bytes each) and store it in a StringBuilder
    Extract separate server and HTTP classes
    Parse the HTTP request
    Create routing table which should allow various HTTP methods
    Make sure the HTTP server can populate the routing table
    Create specific HTTP response classes - TextResponse, for example
    Implement the ToString method for the HTTP response class
    Implement the routing table for storing and retrieving request mapping
    Use the routing table in the HTTP server for actual request-response matching and execution
    Separate the URL and parse the query string if it exists
    Introduce the option to use the request by storing request-response functions in the routing table
    Introduce base controllers and extract common functionalities
    Shorten the route syntax and add support for controllers
    Add redirect HTTP response and use the Location header
    Add view response class and reuse functionality from the HTML response for setting the content
    Add functionality to find specific views by path and by convention
    Make sure the project copies the view files into the output directory
    Add functionality in the base controller class to get the view and controller names by convention
    Add functionality to parse the request form when the specific content type is present
    Add functionality for extracting model data via reflection and replacing it in the HTML
    Add functionality for storing and retrieving HTTP cookies
    Add functionality for storing HTTP session
    Add global exception handling and log all requests and responses in the console
    Use session to store the currently authenticated user ID and write helper methods for authentication
    Add static files option by choosing a public folder and adding all files in it as GET requests in the route table
    Make sure the HTTP server handles byte array response bodies
    Add automatic controller discovery by using reflection and mapping all public methods into the route table by convention
    Add HttpGet and HttpPost attributes to automatically register the HTTP method of the action
    Add Authorize attribute and short-circuit the request if there is no authenticated user
    Implement a layout page logic and insert the view content in it
    Use reflection to analyze the action parameters and populate them automatically from the request
    Remove the required constructor on the base Controller class and populate the request property automatically
    Introduce proper collections for headers, cookies, form and query
    Implement service collection with configuration and create controllers by using a dependendency resolver with reflection and recursion
    Add advanced view engine features - conditionals statements and loops
    Introduce user information during view rendering

Potential Tasks:

    Introduce model state and automatic validation
    Cache
    Include the view files into the assemblies
    Allow headers with the same name
    Make the setter of the controller request private
    Introduce server features to separate the abstraction for routing and services
