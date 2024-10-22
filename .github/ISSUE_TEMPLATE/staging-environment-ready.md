
---
name: Staging Environment Ready for Testing
about: Notify QA that the staging environment is ready for testing
title: "Staging Environment Ready for Testing"
labels: staging, ready-for-testing
assignees: "@tsviz"
---

The staging environment is ready for testing. Please verify that the following items are working as expected:

- [ ] The staging environment is up and running
- [ ] The staging environment is accessible
- [ ] The staging environment is using the correct image tag
- [ ] The staging environment is using the correct database schema

Additional Information:
- Image Tag: `{{ secrets.IMAGE_TAG }}`
- Server URL: `{{ env.CONTAINER_APP_URL }}`
- Terraform IaC files - Artifact URL: `{{ env.ARTIFACT_URL }}`