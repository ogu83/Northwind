"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import axios from "axios";
import Link from "next/link";
import { useState } from "react";

export default function Orders() {
  const queryClient = useQueryClient();

  // pagination state
  const [pageIndex, setPageIndex] = useState(1); // 1-based
  const [pageSize, setPageSize] = useState(10); // items per page
  const pageSizes = [5, 10, 15, 20, 30, 50, 100];

  // order state
  const [orderBy, setOrderBy] = useState("orderId");
  const [isAscending, setIsAscending] = useState(true);

  // Fetch paged orders
  const {
    data: paged,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["orders", pageIndex, pageSize, orderBy, isAscending],
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
        `http://localhost:5205/Order/skip/${skip}/take/${take}/orderby/${orderBy}/asc/${isAscending}`
      );
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await axios.delete(`http://localhost:5205/Order?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["orders", pageIndex] });
    },
    onError: (err) => {
      console.error("Error deleting order:", err);
      alert("Failed to delete order.");
    },
  });

  const handleDelete = (id: number) => {
    if (confirm(`Are you sure you want to delete order ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  const handleOrderBy = (orderby: string) => {
    if (orderBy === orderby) setIsAscending(!isAscending);
    else setOrderBy(orderby);
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading orders</div>;

  const { items: orders, isLastPage } = paged!;
  const totalCount = paged!.totalCount;
  const pageCount = Math.ceil(totalCount / pageSize);

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Orders</h1>
      <div className="flex items-stretch mb-4">
        <Link className="text-blue-500 px-5 cursor-pointer" href="/">
          Home
        </Link>
        <Link className="text-blue-500 px-5 cursor-pointer" href="/orders/add">
          Add New
        </Link>
      </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-center p-2">
              <button
                className="border rounded p-1 cursor-pointer"
                onClick={() => handleOrderBy("orderId")}
              >
                ID
                {orderBy == "orderId" ? (
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
                onClick={() => handleOrderBy("customerId")}
              >
                Customer Id
                {orderBy == "customerId" ? (
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
            <th className="text-left p-2">Employee Id</th>
            <th className="text-left p-2">Order Date</th>
            <th className="text-left p-2">Required Date</th>
            <th className="text-left p-2">Shipped Date</th>
            <th className="text-left p-2">Ship Via</th>
            <th className="text-left p-2">Freight</th>
            <th className="text-left p-2">Ship Name</th>
            <th className="text-left p-2">Ship Address</th>
            <th className="text-left p-2">Ship City</th>
            <th className="text-left p-2">Ship Region</th>
            <th className="text-left p-2">Ship Postal Code</th>
            <th className="text-left p-2">Ship Country</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.orderId} className="border-b hover:bg-orange-950">
              <td className="p-1">{order.orderId}</td>
              <td className="p-1">
                <Link
                  href={`/customers/edit/${order.customerId}`}
                  className="text-blue-500"
                >
                  {order.customerId}
                </Link>
              </td>
              <td className="p-1">{order.employeeId}</td>
              <td className="p-1">
                {new Date(order.orderDate).toLocaleDateString()}
              </td>
              <td className="p-1">
                {new Date(order.requiredDate).toLocaleDateString()}
              </td>
              <td className="p-1">
                {new Date(order.shippedDate).toLocaleDateString()}
              </td>
              <td className="p-1">{order.shipVia}</td>
              <td className="p-1">{order.freight}</td>
              <td className="p-1">{order.shipName}</td>
              <td className="p-1">{order.shipAddress}</td>
              <td className="p-1">{order.shipCity}</td>
              <td className="p-1">{order.shipRegion}</td>
              <td className="p-1">{order.shipPostalCode}</td>
              <td className="p-1">{order.shipCountry}</td>
              <td className="p-1">
                <Link
                  href={`/orders/edit/${order.orderId}`}
                  className="text-blue-500 p-1"
                >
                  Edit
                </Link>
                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(order.orderId)}
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
