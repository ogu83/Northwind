import Link from "next/link";

export default function Home() {
  return (
    <div className="flex flex-col items-center justify-center h-screen p-4">
      <h1 className="text-2xl font-semibold mb-8">Northwind App</h1>
      <div className="flex flex-col w-full max-w-xs space-y-4">
        <Link
          href="/categories"
          className="block w-full text-center bg-green-500 font-semibold text-white rounded p-3 cursor-pointer"
        >
          Categories
        </Link>
        <Link
          href="/products"
          className="block w-full text-center bg-green-500 font-semibold text-white rounded p-3 cursor-pointer"
        >
          Products
        </Link>
        <Link
          href="/customers"
          className="block w-full text-center bg-green-500 font-semibold text-white rounded p-3 cursor-pointer"
        >
          Customers
        </Link>
      </div>
    </div>
  );
}
