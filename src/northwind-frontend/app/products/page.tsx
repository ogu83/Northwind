"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import axios from "axios";
import Link from "next/link";
import { useState } from "react";

export default function Products() {
  const queryClient = useQueryClient();
  // pagination state
  const [pageIndex, setPageIndex] = useState(1); // 1-based
  const [pageSize, setPageSize] = useState(10); // items per page
  const pageSizes = [5, 10, 15, 20, 30, 50, 100];

  // order state
  const [orderBy, setOrderBy] = useState("productId");
  const [isAscending, setIsAscending] = useState(true);

  // Fetch products
  const {
    data: paged,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["products", pageIndex, pageSize, orderBy, isAscending],
    queryFn: async () => {
      const skip = (pageIndex - 1) * pageSize;
      const take = pageSize;
      const res = await axios.get<{
        totalCount: number;
        items: any[];
        pageCount: number;
        pageIndex: number;
        isLastPage: boolean;
      }>(
        `http://localhost:5205/Product/skip/${skip}/take/${take}/orderby/${orderBy}/asc/${isAscending}`
      );
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await axios.delete(`http://localhost:5205/Product?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
    onError: (err) => {
      console.error("Error deleting product:", err);
      alert("Failed to delete product.");
    },
  });

  const handleDelete = (id: number) => {
    if (confirm(`Are you sure you want to delete product ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  const handleOrderBy = (orderby: string) => {
    if (orderBy === orderby) setIsAscending(!isAscending);
    else setOrderBy(orderby);
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading products</div>;

  const { items: products, isLastPage } = paged!;
  const totalCount = paged!.totalCount;
  const pageCount = Math.ceil(totalCount / pageSize);

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Products</h1>
      <div className="flex items-stretch mb-4">
        <Link className="text-blue-500 px-5 cursor-pointer" href="/">
          Home
        </Link>
        <Link
          className="text-blue-500 px-5 cursor-pointer"
          href="/products/add"
        >
          Add New
        </Link>
      </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-left p-2">
              <button
                className="border rounded p-1 cursor-pointer"
                onClick={() => handleOrderBy("productId")}
              >
                ID
                {orderBy == "productId" ? (
                  isAscending ? (
                    <>&uarr;</>
                  ) : (
                    <>&darr;</>
                  )
                ) : (
                  ""
                )}
              </button>
            </th>
            <th className="text-left p-2">
              <button
                className="border rounded p-1 cursor-pointer"
                onClick={() => handleOrderBy("productName")}
              >
                Product Name
                {orderBy == "productName" ? (
                  isAscending ? (
                    <>&uarr;</>
                  ) : (
                    <>&darr;</>
                  )
                ) : (
                  ""
                )}
              </button>
            </th>
            <th className="text-left p-2">Supplier Id</th>
            <th className="text-left p-2">Category Id</th>
            <th className="text-left p-2">Quantity Per Unit</th>
            <th className="text-left p-2">Unit Price</th>
            <th className="text-left p-2">Units In Stock</th>
            <th className="text-left p-2">UnitsOnOrder</th>
            <th className="text-left p-2">Reorder Level</th>
            <th className="text-left p-2">Discontinued</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr
              key={product.productId}
              className="border-b hover:bg-orange-950"
            >
              <td className="p-1">{product.productId}</td>
              <td className="p-1">{product.productName}</td>
              <td className="p-1">
                <Link
                  href={`/suppliers/edit/${product.supplierId}`}
                  className="text-blue-500"
                >
                  {product.supplierId}
                </Link>
              </td>
              <td className="p-1">
                <Link
                  href={`/categories/edit/${product.categoryId}`}
                  className="text-blue-500"
                >
                  {product.categoryId}
                </Link>
              </td>
              <td className="p-1">{product.quantityPerUnit}</td>
              <td className="p-1">{product.unitPrice}</td>
              <td className="p-1">{product.unitsInStock}</td>
              <td className="p-1">{product.unitsOnOrder}</td>
              <td className="p-1">{product.reorderLevel}</td>
              <td className="p-1">{product.discontinued ? "Yes" : "No"}</td>
              <td className="p-1">
                <Link
                  href={`/products/edit/${product.productId}`}
                  className="text-blue-500 p-1"
                >
                  Edit
                </Link>

                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(product.productId)}
                >
                  Delete
                </button>
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
