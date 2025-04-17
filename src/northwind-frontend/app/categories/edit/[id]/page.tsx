"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";

export default function EditCategory() {
  const { id } = useParams();
  const router = useRouter();

  const [category, setCategory] = useState({
    categoryId: id,
    categoryName: "",
    description: "",
  });

  useEffect(() => {
    axios.get(`http://localhost:5205/Category/${id}`).then((res) => {
      setCategory(res.data);
    });
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await axios.put("http://localhost:5205/Category", category);
    router.push("/categories");
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Edit Category {id}</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">
        <input
          type="text"
          className="border p-2"
          placeholder="Category Name"
          value={category.categoryName || ""}
          onChange={(e) =>
            setCategory({ ...category, categoryName: e.target.value })
          }
        />
        <textarea
          className="border p-2"
          placeholder="Description"
          value={category.description || ""}
          onChange={(e) =>
            setCategory({ ...category, description: e.target.value })
          }
        />
        <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
          Save
        </button>
      </form>
    </div>
  );
}
