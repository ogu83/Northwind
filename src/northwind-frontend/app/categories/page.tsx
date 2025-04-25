"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import axios from "axios";
import Link from "next/link";

export default function Categories() {
  const queryClient = useQueryClient();
  const router = useRouter();
  
  // Fetch categories
  const { data: categories, isLoading, error } = useQuery({
    queryKey: ["categories"],
    queryFn: async () => {
      const res = await axios.get("http://localhost:5205/Category");
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await axios.delete(`http://localhost:5205/Category?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["categories"] });
    },
    onError: (err) => {
      console.error("Error deleting category:", err);
      alert("Failed to delete category.");
    },
  });

  const handleDelete = (id: number) => {
    if (confirm(`Are you sure you want to delete category ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading categories</div>;

  return (
    <div className="p-4">
        <h1 className="text-xl font-semibold mb-4">Categories</h1>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href="/">Home</Link>
            <Link className="text-blue-500 px-5 cursor-pointer" href="/categories/add">Add New</Link>
        </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-left p-2">ID</th>
            <th className="text-left p-2">Name</th>
            <th className="text-left p-2">Description</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((cat) => (
            <tr key={cat.categoryId} className="border-b hover:bg-orange-950">
              <td className="p-1">{cat.categoryId}</td>
              <td className="p-1">{cat.categoryName}</td>
              <td className="p-1">{cat.description}</td>
              <td className="p-1">
                <Link href={`/categories/edit/${cat.categoryId}`} className="text-blue-500 p-1">
                  Edit
                </Link>
                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(cat.categoryId)}
                >
                  Delete
                </button>
                <Link
                  className="text-blue-500 p-1 cursor-pointer"
                  href={`/products/category/${cat.categoryId}`} 
                >
                  Products
                </Link>
            </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
