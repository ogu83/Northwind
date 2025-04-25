"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import axios from "axios";
import Link from "next/link";

export default function Suppliers() {
  const queryClient = useQueryClient();
  const router = useRouter();
  
  // Fetch suppliers
  const { data: suppliers, isLoading, error } = useQuery({
    queryKey: ["suppliers"],
    queryFn: async () => {
      const res = await axios.get("http://localhost:5205/Supplier");
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

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading suppliers</div>;

  return (
    <div className="p-4">
        <h1 className="text-xl font-semibold mb-4">Suppliers</h1>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href="/">Home</Link>
            <Link className="text-blue-500 px-5 cursor-pointer" href="/suppliers/add">Add New</Link>
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
            <th className="text-left p-2">Home Page</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {suppliers.map((supplier) => (
            <tr key={supplier.supplierId} className="border-b hover:bg-orange-950">
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
              <td className="p-1">{supplier.homePage}</td>
              <td className="p-1">
                <Link href={`/suppliers/edit/${supplier.supplierId}`} className="text-blue-500 p-1">
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
    </div>
  );
}
