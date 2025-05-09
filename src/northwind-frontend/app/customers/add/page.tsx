"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";
import Link from "next/link";

export default function AddCustomer() {
  const id  = "";
  const router = useRouter();

const [customer, setCustomer] = useState({
    customerId: "",
    companyName: "",
    contactName: "",
    contactTitle: "",
    address: "",
    city: "",
    region: "",
    postalCode: "",
    country: "",
    phone: "",
    fax: ""
});

  const handleSubmit = async (e: { preventDefault: () => void; }) => {
    e.preventDefault();
    await axios.post("http://localhost:5205/Customer", customer);
    router.push("/customers");
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Add Customer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">
      <input
          type="text"
          className="border p-2"
          placeholder="Customer Id"
          value={customer.customerId || ""}
          onChange={(e) =>
            setCustomer({ ...customer, customerId: e.target.value })
          }
        />

      <input
          type="text"
          className="border p-2"
          placeholder="Company Name"
          value={customer.companyName || ""}
          onChange={(e) =>
            setCustomer({ ...customer, companyName: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Contact Name"
          value={customer.contactName || ""}
          onChange={(e) =>
            setCustomer({ ...customer, contactName: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Contact Title"
          value={customer.contactTitle || ""}
          onChange={(e) =>
            setCustomer({ ...customer, contactTitle: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Address"
          value={customer.address || ""}
          onChange={(e) =>
            setCustomer({ ...customer, address: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="City"
          value={customer.city || ""}
          onChange={(e) =>
            setCustomer({ ...customer, city: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Region"
          value={customer.region || ""}
          onChange={(e) =>
            setCustomer({ ...customer, region: e.target.value })
          }
        />
        
        <input
          type="text"
          className="border p-2"
          placeholder="Postal Code"
          value={customer.postalCode || ""}
          onChange={(e) =>
            setCustomer({ ...customer, postalCode: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Country"
          value={customer.country || ""}
          onChange={(e) =>
            setCustomer({ ...customer, country: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Phone"
          value={customer.phone || ""}
          onChange={(e) =>
            setCustomer({ ...customer, phone: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Fax"
          value={customer.fax || ""}
          onChange={(e) =>
            setCustomer({ ...customer, fax: e.target.value })
          }
        />
        <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded">
          Save
        </button>
        <Link href="/customers" className="bg-blue-500 text-white px-4 py-2 rounded text-center">Cancel</Link>
      </form>
    </div>
  );
}
