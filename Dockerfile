FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Install locales package and generate en_US.UTF-8 locale
RUN apt-get update && apt-get install -y locales \
    && locale-gen en_US.UTF-8 \
    && update-locale LANG=en_US.UTF-8

# Set environment variables for locale
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

# Copy all files in the publish directory
COPY ./publish .

# Expose the ports the app runs on
EXPOSE 80
EXPOSE 1433

# Run the app on container startup
ENTRYPOINT ["/app/RazorPagesMovie"]