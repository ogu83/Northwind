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

type Customer = {
  customerId: string,
  companyName: string
}


export default function AddOrder() {
  const queryClient = useQueryClient();
  const id = 0;
  const router = useRouter();

  const [customers, setCustomers] = useState<Customer[]>([]);

  const [order, setOrder] = useState<Order>({
    orderId: Number(id!),
    customerId: "",
    employeeId: 1,
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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
  
    try {
      // 1) Send the POST and grab the created Order from the response body
      const { data: createdOrder } = await axios.post<Order>(
        "http://localhost:5205/Order",
        {
          ...order,
          orderDate:    order.orderDate.toISOString(),
          requiredDate: order.requiredDate.toISOString(),
          shippedDate:  order.shippedDate.toISOString(),
        }
      );
  
      // 2) Pull the returned orderId…
      const orderId = createdOrder.orderId;
  
      // 3) …and navigate straight to the edit page for that order
      router.push(`/orders/edit/${orderId}`);
    } catch (err) {
      console.error("Failed to create order", err);
      alert("Sorry, we couldn't save your order.");
    }
  };
  

  return (
    <div className="flex flex-col items-top md:flex-row">
    <div className="p-4 w-100">
      <h1 className="text-xl font-semibold mb-4">Add Order</h1>
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
    </div>
  );
}
