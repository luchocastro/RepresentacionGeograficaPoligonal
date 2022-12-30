# Data Module
===========

The data module is a top level directory and holds the schema (models/entities) and services (repositories) for data consumed by the application.

By default there are two subdirectories:
> ~/src/app/data
>   /schema
>   /service


## Multiple Data Sources
If your application consumes data from more than one source then the data directory should be restructured to contain subdirectories for each data source. Do not create multiple modules for each data source:

> ~/src/app/data
>   /data-source-one
>     /schema
>     /service
>   /data-source-two
>     /schema
>     /service
>   /data.module.ts


## Schema Naming Standard
A schema file is very much like an entity file in an Object Relational Mapper. This schema file is central to your application’s consumption of data and therefore does not need cursory decorators such as calling it ProjectSchema or ProjectModel. Schemas are special because they are the only plain-named class in the application.