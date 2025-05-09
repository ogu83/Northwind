"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";
import DatePicker from "react-datepicker";
import Link from "next/link";

type Order = {
    orderId: number,
    customerId: string,
    employeeId: number,
    orderDate: Date,
    requiredDate: Date,
    shippedDate: Date,
    shipVia: number,
    freight: number,
    shipName: string,
    shipAddress: string,
    shipCity: string,
    shipRegion: string,
    shipPostalCode: string,
    shipCountry: string
};

type OrderDetail = {
    orderId: number,
    productId: number,
    unitPrice: number,
    quantity: number,
    discount: number
}

type Customer = {
  customerId: string,
  companyName: string
}

export default function EditOrder() {
  const queryClient = useQueryClient();
  const { id } = useParams();
  const router = useRouter();

  const [customers, setCustomers] = useState<Customer[]>([]);

  const [order, setOrder] = useState<Order>({
    orderId: Number(id!),
    customerId: "",
    employeeId: 0,
    orderDate: new Date(),
    requiredDate: new Date(),
    shippedDate: new Date(),
    shipVia: 0,
    freight: 0,
    shipName: "",
    shipAddress: "",
    shipCity: "",
    shipRegion: "",
    shipPostalCode: "",
    shipCountry: ""
  });

  // fetch customers list
  useEffect(() => {
    axios.get<Customer[]>(`http://localhost:5205/Customer`).then((res) => {
        setCustomers(res.data);
    });
  }, []);

  // fetch order
  useEffect(() => {
    axios.get(`http://localhost:5205/Order/${id}`).then((res) => {
      const d = res.data;
      setOrder({
        ...d,
        // wrap incoming values in Date()
        orderDate:    new Date(d.orderDate),
        requiredDate: new Date(d.requiredDate),
        shippedDate:  new Date(d.shippedDate),
      });
    });
  }, [id]);

  // Fetch order details
  const { 
    data: orderdetails = [],    // ← defaults to [] instead of undefined
    isLoading,
    error
  } = useQuery({
    queryKey: ["orderdetails", id],    // include `id` so it refetches when the order changes
    queryFn: async () => {
      const res = await axios.get<OrderDetail[]>(`http://localhost:5205/OrderDetails/Order/${id}`);
      return res.data;
    },
  });

  const handleSubmit = async (e: { preventDefault: () => void; }) => {
    e.preventDefault();
    await axios.put("http://localhost:5205/Order", {
        ...order,
        orderDate: order.orderDate.toISOString(),
        requiredDate: order.requiredDate.toISOString(),
        shippedDate: order.shippedDate.toISOString(),
      });
    router.push("/orders");
  }

  // Delete mutation
  const deleteMutation = useMutation<
    void,                        // return type
    unknown,                     // error type
    { orderId: number; productId: number }  // variables
    >({
    mutationFn: async ({ orderId, productId }) => {
      await axios.delete(
        "http://localhost:5205/OrderDetails",
        {
          data: {
            item1: orderId,
            item2: productId,
          },
        }
      );
    },
    onSuccess: () => {
        // include the order `id` in the key so it refetches the right list
        queryClient.invalidateQueries({ queryKey: ["orderdetails", id] });
    },
    onError: (err) => {
        console.error("Error deleting order detail:", err);
        alert("Failed to delete order detail.");
    },
    });
  const handleDetailDelete = (orderId: number, productId: number) => {
      if (confirm(`Are you sure you want to delete order detail ${productId}?`)) {
        deleteMutation.mutate({ orderId, productId });
      }
  };

  return (
    <div className="flex flex-col items-top md:flex-row">
    <div className="p-4 w-100">
      <h1 className="text-xl font-semibold mb-4">Edit Order {id}</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">

        {/* Customer Dropdown */}
        <select
          className="border p-2"
          value={order.customerId}
          onChange={(e) =>
            setOrder({ ...order, customerId: e.target.value })
          }
        >
          <option value={0} disabled>
            -- Select Customer --
          </option>
          {customers.map((s) => (
            <option key={s.customerId} value={s.customerId}>
              {s.customerId} – {s.companyName}
            </option>
          ))}
        </select>

        <div>
          <label className="block mb-1 text-xs">Order Date</label>
          <DatePicker
            selected={order.orderDate}
            onChange={(date) =>
              setOrder((o) => ({ ...o, orderDate: date! }))
            }
            dateFormat="MM/dd/yyyy"
            className="
            border p-2 w-full
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
          "
          calendarClassName="
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
            shadow-lg
          "
          dayClassName={() =>
            "hover:bg-gray-200 dark:hover:bg-gray-700"
          }
          />
        </div>
        <div>
          <label className="block mb-1 text-xs">Required Date</label>
          <DatePicker
            selected={order.requiredDate}
            onChange={(date) =>
              setOrder((o) => ({ ...o, requiredDate: date! }))
            }
            dateFormat="MM/dd/yyyy"
            className="
            border p-2 w-full
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
          "
          calendarClassName="
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
            shadow-lg
          "
          dayClassName={() =>
            "hover:bg-gray-200 dark:hover:bg-gray-700"
          }
          />
        </div>
        <div>
          <label className="block mb-1 text-xs">Shipped Date</label>
          <DatePicker
            selected={order.shippedDate}
            onChange={(date) =>
              setOrder((o) => ({ ...o, shippedDate: date! }))
            }
            dateFormat="MM/dd/yyyy"
            className="
            border p-2 w-full
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
          "
          calendarClassName="
            bg-white text-black
            dark:bg-gray-800 dark:text-gray-100
            shadow-lg
          "
          dayClassName={() =>
            "hover:bg-gray-200 dark:hover:bg-gray-700"
          }
          />
        </div>

        <input
          type="number"
          className="border p-2"
          placeholder="Ship Via"
          value={order.shipVia || ""}
          onChange={(e) =>
            setOrder({ ...order, shipVia: Number(e.target.value) })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Freight"
          value={order.freight || ""}
          onChange={(e) =>
            setOrder({ ...order, freight: Number(e.target.value) })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Ship Name"
          value={order.shipName || ""}
          onChange={(e) =>
            setOrder({ ...order, shipName: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Ship Address"
          value={order.shipAddress || ""}
          onChange={(e) =>
            setOrder({ ...order, shipAddress: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Ship Region"
          value={order.shipRegion || ""}
          onChange={(e) =>
            setOrder({ ...order, shipRegion: e.target.value })
          }
        />
        
        <input
          type="text"
          className="border p-2"
          placeholder="Ship City"
          value={order.shipCity || ""}
          onChange={(e) =>
            setOrder({ ...order, shipCity: e.target.value })
          }
        />
               
        <input
          type="text"
          className="border p-2"
          placeholder="Ship Postal Code"
          value={order.shipPostalCode || ""}
          onChange={(e) =>
            setOrder({ ...order, shipPostalCode: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Ship Country"
          value={order.shipCountry || ""}
          onChange={(e) =>
            setOrder({ ...order, shipCountry: e.target.value })
          }
        />

        <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded">
          Save
        </button>
        <Link href="/orders" className="bg-blue-500 text-white px-4 py-2 rounded text-center">Cancel</Link>
      </form>
    </div>
    <div className="p-4 w-200">
        <h2 className="text-xl mb-4">Order Details</h2>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href={`/orders/edit/${id}/details/add`}>Add New</Link>
        </div>
        <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
            <th className="text-left p-2">Product Id</th>
            <th className="text-left p-2">Unit Price</th>
            <th className="text-left p-2">Quantity</th>
            <th className="text-left p-2">Discount</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {orderdetails!.map((orderdetail) => (
            <tr key={`${orderdetail.orderId}-${orderdetail.productId}`} className="border-b hover:bg-orange-950">
              <td className="p-1"><Link className="text-blue-500" href={`/products/edit/${orderdetail.productId}`}>{orderdetail.productId}</Link></td>
              <td className="p-1">{orderdetail.unitPrice}</td>
              <td className="p-1">{orderdetail.quantity}</td>
              <td className="p-1">{orderdetail.discount}</td>
              <td className="p-1">
                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDetailDelete(orderdetail.orderId, orderdetail.productId)}
                >
                  Delete
                </button>
            </td>
            </tr>
          ))}
        </tbody>
      </table>
      </div>
    </div>
  );
}
