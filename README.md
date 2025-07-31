# study-buddy-gpt


Embedding Server Setup 

----------------------

This is a local service running on Python



Step 1: Python Environment

1. Install Python 3.10+
2. Create virtual env (python -m venv rag-embedder)
3. Activate it (.\\rag-embedder\\Scripts\\activate)
4. Install dependencies (pip install fastapi uvicorn sentence-transformers)



Step 2: Run the server

1. Navigate to "embed-server-scripts"
2. Run the script (uvicorn embed\_server:app --host 127.0.0.1 --port 8000)





Vector Database Setup
---------------------

Qdrant is running locally on Docker. 


Step 1: Install Docker

Step 2: Run Qdrant Container
	docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant
	(use the command "docker run -d -p 6333:6333 -p 6334:6334 qdrant/qdrant" to run in background mode)


LLM Setup
----------
Ollama needs to be running locally.

Step 1: Setup Ollama
1. Install Ollama (https://ollama.com)

