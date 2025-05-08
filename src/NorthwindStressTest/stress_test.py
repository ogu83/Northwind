import requests
import random
import time
import concurrent.futures

BASE_URL = "http://localhost:5205"

def list_orders(skip, take):
    """Fetch a paginated list of orders."""
    response = requests.get(f"{BASE_URL}/Order/skip/{skip}/take/{take}")
    response.raise_for_status()
    return response.json()

def get_random_page(take):
    """Pick a random page of orders."""
    # First, fetch page 0 to get pagination info
    first_page = list_orders(0, take)
    total_count = first_page['totalCount']
    page_count = first_page['pageCount']
    page_index = random.randint(0, page_count - 1)
    skip = page_index * take
    page = list_orders(skip, take)
    return page

def update_order_random(order):
    """Edit an order with random changes and save."""
    original = order.copy()
    # Randomly modify freight for example
    modified = order.copy()
    modified['freight'] = (modified.get('freight') or 0) + round(random.uniform(1, 10), 2)
    requests.put(f"{BASE_URL}/Order", json=modified).raise_for_status()
    return original

def create_order(order):
    """Create a new order."""
    new_order = order.copy()
    new_order['orderId'] = 0  # Let the API assign a new ID
    response = requests.post(f"{BASE_URL}/Order", json=new_order)
    response.raise_for_status()
    return response.json()

def find_order(order_id, take):
    """Search paginated lists to find the created order."""
    skip = 0
    while True:
        page = list_orders(skip, take)
        for o in page['items']:
            if o['orderId'] == order_id:
                return o
        skip += take
        if skip >= page['totalCount']:
            return None

def delete_order(order_id):
    """Delete an order by ID."""
    requests.delete(f"{BASE_URL}/Order?id={order_id}").raise_for_status()

def user_story(take=10):
    """Perform the sequence: list, update, revert, create, find, delete."""
    # 1. List and pick random
    page = get_random_page(take)
    if not page['items']:
        return
    order = random.choice(page['items'])

    # 2. Edit order
    original = update_order_random(order)
    time.sleep(0.1)

    # 3. Re-fetch and revert
    updated = requests.get(f"{BASE_URL}/Order/{order['orderId']}").json()
    requests.put(f"{BASE_URL}/Order", json=original).raise_for_status()

    # 4. Create new order
    created = create_order(updated)
    new_id = created['orderId']

    # 5. Find created order
    found = find_order(new_id, take)
    print(f"Order {new_id} found: {found is not None}")

    # 6. Delete the order
    delete_order(new_id)
    print(f"Order {new_id} deleted.")

def main(concurrency=5, iterations=10):
    """Run multiple user stories concurrently."""
    with concurrent.futures.ThreadPoolExecutor(max_workers=concurrency) as executor:
        futures = [executor.submit(user_story) for _ in range(iterations)]
        concurrent.futures.wait(futures)

if __name__ == "__main__":
    main()
