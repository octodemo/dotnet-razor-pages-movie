FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Install locales package and generate en_US.UTF-8 locale
RUN apt-get update && apt-get install -y locales \
    && locale-gen en_US.UTF-8 \
    && locale-gen LANG=en_US.UTF-8

# Set environment variables for locale
ENV LANG=en_US.UTF-8
ENV LANGUAGE=en_US:en
ENV LC_ALL=en_US.UTF-8

# Set Azure Data Protection key ring environment variables for session reliability
ENV AZURE_BLOB_KEYRING_CONNECTION_STRING="DefaultEndpointsProtocol=https;AccountName=razorpagesmoviestorage;AccountKey=kOXkWNLVhNZk/dTU6bH6ZOcrPrpeC13gms2XdOA/fqAGB+sUNgqnjI4yn07ODMkbacJxlL2oqliF+AStuZxNxw==;EndpointSuffix=core.windows.net"
ENV AZURE_BLOB_KEYRING_CONTAINER="dataprotection"
# ENV AZURE_BLOB_KEYRING_BLOB="dataprotection-keys.xml"  # Optional, default is fine

# Copy all files in the publish directory
COPY ./publish .

# Run chmod to make the app executable
RUN chmod +x /app/RazorPagesMovie

# Expose the ports the app runs on
EXPOSE 80
EXPOSE 1433

# Run the app on container startup
ENTRYPOINT ["/app/RazorPagesMovie"]