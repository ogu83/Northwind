"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import axios from "axios";
import Link from "next/link";
import { useState } from "react";

export default function Suppliers() {
  function renderHomePageLink(raw: string | null) {
    if (!raw) {
      return <span className="text-gray-500">—</span>;
    }

    const parts = raw.split("#");
    // parts = ["", "CAJUN.HTM", ""] when raw === "#CAJUN.HTM#"
    if (parts.length >= 2) {
      // use the URL itself as display text if none was provided
      const url = parts[1];
      const text = parts[0] || url;

      if (url) {
        return (
          <a
            href={url}
            target="_blank"
            rel="noopener noreferrer"
            className="text-blue-500 underline"
          >
            {text}
          </a>
        );
      }
    }

    // fallback if it doesn’t match the pattern
    return raw;
  }

  const queryClient = useQueryClient();

  // pagination state
  const [pageIndex, setPageIndex] = useState(1); // 1-based
  const [pageSize, setPageSize] = useState(10); // items per page
  const pageSizes = [5, 10, 15, 20, 30, 50, 100];

  // order state
  const [orderBy, setOrderBy] = useState("supplierId");
  const [isAscending, setIsAscending] = useState(true);

  // Fetch suppliers
  const {
    data: paged,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["suppliers", pageIndex, pageSize, orderBy, isAscending],
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
        `http://localhost:5205/Supplier/skip/${skip}/take/${take}/orderby/${orderBy}/asc/${isAscending}`
      );
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      await axios.delete(`http://localhost:5205/Supplier?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["suppliers"] });
    },
    onError: (err) => {
      console.error("Error deleting supplier:", err);
      alert("Failed to delete supplier.");
    },
  });

  const handleDelete = (id: string) => {
    if (confirm(`Are you sure you want to delete supplier ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  const handleOrderBy = (orderby: string) => {
    if (orderBy === orderby) setIsAscending(!isAscending);
    else setOrderBy(orderby);
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading suppliers</div>;

  const { items: suppliers, isLastPage } = paged!;
  const totalCount = paged!.totalCount;
  const pageCount = Math.ceil(totalCount / pageSize);

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Suppliers</h1>
      <div className="flex items-stretch mb-4">
        <Link className="text-blue-500 px-5 cursor-pointer" href="/">
          Home
        </Link>
        <Link
          className="text-blue-500 px-5 cursor-pointer"
          href="/suppliers/add"
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
                onClick={() => handleOrderBy("supplierId")}
              >
                ID
                {orderBy == "supplierId" ? (
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
                onClick={() => handleOrderBy("companyName")}
              >
                Company Name
                {orderBy == "companyName" ? (
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
                onClick={() => handleOrderBy("contactName")}
              >
                Contact Name
                {orderBy == "contactName" ? (
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
                onClick={() => handleOrderBy("contactTitle")}
              >
                Contact Title
                {orderBy == "contactTitle" ? (
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
                onClick={() => handleOrderBy("address")}
              >
                Address
                {orderBy == "address" ? (
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
                onClick={() => handleOrderBy("city")}
              >
                City
                {orderBy == "city" ? (
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
                onClick={() => handleOrderBy("region")}
              >
                Region
                {orderBy == "region" ? (
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
                onClick={() => handleOrderBy("postalCode")}
              >
                Postal Code
                {orderBy == "postalCode" ? (
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
                onClick={() => handleOrderBy("country")}
              >
                Country
                {orderBy == "country" ? (
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
            <th className="text-left p-2">Phone</th>
            <th className="text-left p-2">Fax</th>
            <th className="text-left p-2">Home Page</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {suppliers.map((supplier) => (
            <tr
              key={supplier.supplierId}
              className="border-b hover:bg-orange-950"
            >
              <td className="p-1">{supplier.supplierId}</td>
              <td className="p-1">{supplier.companyName}</td>
              <td className="p-1">{supplier.contactName}</td>
              <td className="p-1">{supplier.contactTitle}</td>
              <td className="p-1">{supplier.address}</td>
              <td className="p-1">{supplier.city}</td>
              <td className="p-1">{supplier.region}</td>
              <td className="p-1">{supplier.postalCode}</td>
              <td className="p-1">{supplier.country}</td>
              <td className="p-1">{supplier.phone}</td>
              <td className="p-1">{supplier.fax}</td>
              <td className="p-1">{renderHomePageLink(supplier.homePage)}</td>
              <td className="p-1">
                <Link
                  href={`/suppliers/edit/${supplier.supplierId}`}
                  className="text-blue-500 p-1"
                >
                  Edit
                </Link>
                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(supplier.supplierId)}
                >
                  Delete
                </button>
                <Link
                  className="text-blue-500 p-1 cursor-pointer"
                  href={`/products/supplier/${supplier.supplierId}`}
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
