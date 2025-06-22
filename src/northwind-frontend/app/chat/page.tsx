"use client";
import Link from "next/link";
import React, { useEffect, useRef, useState } from "react";

// Change to your backend API root
const API_BASE = "http://localhost:5208";

type ChatMessage = {
  authorName?: string;
  role?: { value: string };
  contents?: any[];
};

export default function ChatPage() {
  const [sessionId, setSessionId] = useState<string | null>(null);
  const [history, setHistory] = useState<ChatMessage[]>([]);
  const [prompt, setPrompt] = useState("");
  const [loading, setLoading] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);

  // Get a session ID on mount
  useEffect(() => {
    fetch(`${API_BASE}/Chat/CreateSession`)
      .then((res) => res.json())
      .then((obj) => setSessionId(obj.sessionId))
      .catch(console.error);
  }, []);

  // Handles sending a prompt
  async function handleSend(e?: React.FormEvent) {
    if (e) e.preventDefault();
    if (!prompt.trim() || !sessionId) return;
    setLoading(true);

    try {
      const res = await fetch(`${API_BASE}/Chat/Prompt/${sessionId}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(prompt.trim()),
      });
      if (!res.ok) throw new Error("Failed to send prompt.");
      const reply = await res.json();
      // Append both user and bot messages to history
      setHistory((prev) => [
        ...prev,
        { role: { value: "user" }, contents: [{ text: prompt }] },
        reply,
      ]);
      setPrompt("");
      inputRef.current?.focus();
    } catch (err) {
      alert((err as Error).message);
    }
    setLoading(false);
  }

  // Helper for rendering messages
  function renderMsg(msg: ChatMessage, idx: number) {
    const who =
      msg.role?.value === "user"
        ? "You"
        : msg.role?.value
        ? msg.role.value
        : "AI";
    let text =
      msg.contents && msg.contents.length
        ? msg.contents
            .map((c: any) => c.text || JSON.stringify(c))
            .join("<br/>") // join with <br/> for line breaks if needed
        : "";

    return (
      <div
        key={idx}
        className={
          "mb-2 p-2 rounded " +
          (msg.role?.value === "user"
            ? "bg-blue-500 text-white text-right"
            : "bg-gray-500 text-white text-left")
        }
      >
        <div className="font-semibold">{who}</div>
        <div dangerouslySetInnerHTML={{ __html: text }} />
      </div>
    );
  }

  return (
    <div className="max-w-xxl mx-auto p-4">
      <div className="flex items-center mb-4">
        <Link className="text-blue-500 px-5 cursor-pointer" href="/">
          Home
        </Link>
        <h2 className="font-bold text-xl mb-2">Chat with MCP Agent</h2>
      </div>
      {sessionId && (
        <div className="text-gray-500 mb-2">
          Session ID: <span className="font-mono">{sessionId}</span>
        </div>
      )}
      {!sessionId && <div>Initializing chat session…</div>}
      <div className="border rounded p-2 h-auto overflow-y-auto bg-black mb-2 shadow">
        {history.length === 0 && (
          <div className="text-gray-400 italic">No conversation yet.</div>
        )}
        {history.map(renderMsg)}
        {loading && <div className="text-gray-400">Thinking…</div>}
      </div>
      <form className="flex" onSubmit={handleSend}>
        <input
          ref={inputRef}
          className="border p-2 rounded-l flex-1"
          type="text"
          value={prompt}
          placeholder="Type your prompt..."
          disabled={loading || !sessionId}
          onChange={(e) => setPrompt(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter" && !e.shiftKey) handleSend(e);
          }}
        />
        <button
          type="submit"
          className="bg-blue-600 text-white px-4 rounded-r disabled:bg-gray-300"
          disabled={loading || !prompt.trim() || !sessionId}
        >
          Send
        </button>
      </form>
    </div>
  );
}
