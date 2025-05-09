"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import axios from "axios";
import Link from "next/link";

export default function Orders() {
  const queryClient = useQueryClient();
  const router = useRouter();
  const { id } = useParams();
  
  // Fetch orders
  const { data: orders, isLoading, error } = useQuery({
    queryKey: ["orders"],
    queryFn: async () => {
      const res = await axios.get(`http://localhost:5205/Order/Customer/${id}`);
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      await axios.delete(`http://localhost:5205/Order?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["orders"] });
    },
    onError: (err) => {
      console.error("Error deleting order:", err);
      alert("Failed to delete order.");
    },
  });

  const handleDelete = (id: string) => {
    if (confirm(`Are you sure you want to delete order ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading orders</div>;

  return (
    <div className="p-4">
        <h1 className="text-xl font-semibold mb-4">Orders By Customer {id}</h1>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href="/">Home</Link>
            <Link className="text-blue-500 px-5 cursor-pointer" href="/orders/add">Add New</Link>
        </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-left p-2">ID</th>
            <th className="text-left p-2">Customer Id</th>
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
          {orders.map((order: any) => (
            <tr key={order.orderId} className="border-b hover:bg-orange-950">
              <td className="p-1">{order.orderId}</td>
              <td className="p-1">
                <Link href={`/customers/edit/${order.customerId}`} className="text-blue-500">{order.customerId}</Link>
              </td>
              <td className="p-1">{order.employeeId}</td>
              <td className="p-1">{new Date(order.orderDate).toLocaleDateString()}</td>
              <td className="p-1">{new Date(order.requiredDate).toLocaleDateString()}</td>
              <td className="p-1">{new Date(order.shippedDate).toLocaleDateString()}</td>
              <td className="p-1">{order.shipVia}</td>
              <td className="p-1">{order.freight}</td>
              <td className="p-1">{order.shipName}</td>
              <td className="p-1">{order.shipAddress}</td>
              <td className="p-1">{order.shipCity}</td>
              <td className="p-1">{order.shipRegion}</td>
              <td className="p-1">{order.shipPostalCode}</td>
              <td className="p-1">{order.shipCountry}</td>
              <td className="p-1">
                <Link href={`/orders/edit/${order.orderId}`} className="text-blue-500 p-1">
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
    </div>
  );
}
