"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import axios from "axios";
import Link from "next/link";

export default function Customers() {
  const queryClient = useQueryClient();
  const router = useRouter();
  
  // Fetch customers
  const { data: customers, isLoading, error } = useQuery({
    queryKey: ["customers"],
    queryFn: async () => {
      const res = await axios.get("http://localhost:5205/Customer");
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      await axios.delete(`http://localhost:5205/Customer?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["customers"] });
    },
    onError: (err) => {
      console.error("Error deleting customer:", err);
      alert("Failed to delete customer.");
    },
  });

  const handleDelete = (id: string) => {
    if (confirm(`Are you sure you want to delete customer ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading customers</div>;

  return (
    <div className="p-4">
        <h1 className="text-xl font-semibold mb-4">Customers</h1>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href="/">Home</Link>
            <Link className="text-blue-500 px-5 cursor-pointer" href="/customers/add">Add New</Link>
        </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-left p-2">ID</th>
            <th className="text-left p-2">Company Name</th>
            <th className="text-left p-2">Contact Name</th>
            <th className="text-left p-2">Contact Title</th>
            <th className="text-left p-2">Address</th>
            <th className="text-left p-2">City</th>
            <th className="text-left p-2">Region</th>
            <th className="text-left p-2">Postal Code</th>
            <th className="text-left p-2">Country</th>
            <th className="text-left p-2">Phone</th>
            <th className="text-left p-2">Fax</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {customers.map((customer) => (
            <tr key={customer.customerId} className="border-b hover:bg-gray-100">
              <td className="p-2">{customer.customerId}</td>
              <td className="p-2">{customer.companyName}</td>
              <td className="p-2">{customer.contactName}</td>
              <td className="p-2">{customer.contactTitle}</td>
              <td className="p-2">{customer.address}</td>
              <td className="p-2">{customer.city}</td>
              <td className="p-2">{customer.region}</td>
              <td className="p-2">{customer.postalCode}</td>
              <td className="p-2">{customer.country}</td>
              <td className="p-2">{customer.phone}</td>
              <td className="p-2">{customer.fax}</td>
              <td className="p-2">
                <Link href={`/customers/edit/${customer.customerId}`} className="text-blue-500 p-1">
                  Edit
                </Link>

                <Link
                  className="text-blue-500 p-1 cursor-pointer"
                  href={`/order/customer/${customer.customerId}`} 
                >
                  Orders
                </Link>

                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(customer.customerId)}
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
