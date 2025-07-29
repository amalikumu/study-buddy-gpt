from fastapi import FastAPI, Request
from sentence_transformers import SentenceTransformer
from pydantic import BaseModel

app = FastAPI()
model = SentenceTransformer("all-MiniLM-L6-v2")

class TextRequest(BaseModel):
    texts: list[str]

@app.post("/embed")
def embed(request: TextRequest):
    embeddings = model.encode(request.texts).tolist()
    return {"embeddings": embeddings}