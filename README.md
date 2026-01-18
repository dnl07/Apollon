# Apollon

## What is Apollon?

Apollon (named after the Greek god of prophecy) is a lightweight, fast and ease-to-use search engine that can be run as an HTTP service and intergrated into your own applications.

It provides simple document ranking based on relevance and supports approximate string matching (fuzzy search), making it well-suited for small to medium-sized projects that need flexible and efficient full-text search without heavy dependencies.

## Features 

- Fast full-text search
- BM25-based ranking
- Fuzzy search (edit distance)
- Document management (add, update, delete)
- Lightweight, no heavy dependencies

## API Endpoints

### Documents
Endpoints:
- ```POST /document/add```- Adds a single document
- ```POST /document/bulk```- Adds multiple documents at once
- ```PUT /document/update/{id}```- Updates a document
- ```DELETE /document/delete/{id}```- Deletes a document

Document JSON structure:
```
{
    "title": "string",
    "description": "string",
    "tags": ["string", "string"]
}
```

### Seach
Endpoints:
- ```GET /search?q=...``` - Simple search
- ```GET /search?q=...&explain=true``` - Simple search with explanation
- ```POST /search``` . Search with custom options

Example response:
```
{
    "query": "string",
    "total": 0,
    "tookMs": 0,
    "hits": [{
        "id": 0,
        "fields": {
            "title": "string",
            "description": "string",
            "tags": "string",        
        }
    }]
}
```
Example response with explanation:
```
{
    ...
    "hits": [{
        "id": 0,
        "fields": {
            "title": "string",
            "description": "string",
            "tags": "string",        
        },
        "explain": {
            "bm25": {
                "score": 0,
                "tf": 0,
                "idf": 0,
            },
            "finalScore": 0
        }
    }]
}
```
Search with options:
```
{
    "query": "string",
    "options": {
        TODO
    }
}
```

### Engine Configuration
- ```POST /engine/init```- Initializes the search engine with custom options (should be done before adding documents)
TODO: example json
- TODO: ```GET /engine/status``` - Returns the status of the engine and additional information about the used structures (like index size or total number of documents)

### Health
- ```GET /health``` - Returns basic status of the search engine

## How does it work?
### BM25-Scoring
TODO
### Fuzzy Search
TODO