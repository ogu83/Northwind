"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useState } from "react";
import axios from "axios";
import Link from "next/link";

export default function Categories() {
  const queryClient = useQueryClient();

  // pagination state
  const [pageIndex, setPageIndex] = useState(1); // 1-based
  const [pageSize, setPageSize] = useState(10); // items per page
  const pageSizes = [5, 10, 15, 20, 30, 50, 100];

  // Fetch categories
  const {
    data: paged,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["orders", pageIndex, pageSize],
    queryFn: async () => {
      const skip = (pageIndex - 1) * pageSize;
      const take = pageSize;
      const res = await axios.get<{
        items: any[];
        pageCount: number;
        pageIndex: number;
        isLastPage: boolean;
      }>(`http://localhost:5205/Category/skip/${skip}/take/${take}`);
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

  const { items: categories, isLastPage } = paged!;
  const totalCount = paged!.totalCount;
  const pageCount = Math.ceil(totalCount / pageSize);

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Categories</h1>
      <div className="flex items-stretch mb-4">
        <Link className="text-blue-500 px-5 cursor-pointer" href="/">
          Home
        </Link>
        <Link
          className="text-blue-500 px-5 cursor-pointer"
          href="/categories/add"
        >
          Add New
        </Link>
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
                <Link
                  href={`/categories/edit/${cat.categoryId}`}
                  className="text-blue-500 p-1"
                >
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

      {/* Pagination controls */}
      <div className="flex flex-wrap gap-2 items-center m-2">
        {/* Page size selector */}
        <div className="flex items-center items-center">
          <label htmlFor="pageSize" className="mr-1 w-20">
            Page Size
          </label>
          <select
            id="pageSize"
            value={pageSize}
            onChange={(e) => {
              setPageSize(Number(e.target.value));
              setPageIndex(1);
            }}
            className="border px-1"
          >
            {pageSizes.map((size) => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
        </div>

        <button
          onClick={() => setPageIndex((i) => Math.max(1, i - 1))}
          disabled={pageIndex === 1}
          className="px-2 border rounded disabled:opacity-50"
        >
          Prev
        </button>

        {/* page buttons */}
        {Array.from({ length: pageCount }, (_, idx) => {
          const page = idx + 1;
          return (
            <button
              key={page}
              onClick={() => setPageIndex(page)}
              className={`px-2 border rounded ${
                page === pageIndex ? "bg-blue-700" : ""
              }`}
            >
              {page}
            </button>
          );
        })}

        <button
          onClick={() => setPageIndex((i) => (isLastPage ? i : i + 1))}
          disabled={isLastPage}
          className="px-2 border rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
}
