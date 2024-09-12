FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
# Copy all files in the publish directory
COPY ./publish .
# Ensure the binary has execute permissions
RUN chmod +x /app/RazorPagesMovie
RUN chmod +x /app/RazorPagesMovie.dll
# Expose the ports the app runs on
EXPOSE 80
EXPOSE 1433
# Run the app on container startup
ENTRYPOINT ["/app/RazorPagesMovie"]