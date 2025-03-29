export const mockProductList = [
  {
    id: 1,
    name: "Small Plastic Bike",
    price: 307.95,
    description:
      "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J",
    brand: "Halvorson - Medhurst",
    stockQuantity: 14,
    averageRating: 0,
    isWished: false,
    sold: 44,
    isNew: false,
    images: [],
    categories: [
      {
        id: 2,
        name: "Games, Sports & Beauty",
        description: "Animi distinctio non cum quia quos modi non laborum est.",
      },
      {
        id: 4,
        name: "Baby",
        description:
          "Culpa omnis illo a quia assumenda vel deserunt eos veniam minima dolorem aspernatur.",
      },
    ],
    colors: [],
  },
  {
    id: 2,
    name: "Handcrafted Plastic Shirt",
    price: 651.13,
    description: "The Football Is Good For Training And Recreational Purposes",
    brand: "Mertz - Cole",
    stockQuantity: 87,
    averageRating: 0,
    isWished: false,
    sold: 5,
    isNew: false,
    images: [],
    categories: [
      {
        id: 5,
        name: "Books & Games",
        description: "Beatae incidunt saepe unde laboriosam culpa magnam.",
      },
    ],
    colors: [],
  },
  {
    id: 3,
    name: "Unbranded Concrete Pizza",
    price: 945.25,
    description:
      "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients",
    brand: "Kshlerin, Blanda and Kautzer",
    stockQuantity: 53,
    averageRating: 0,
    isWished: false,
    sold: 9,
    isNew: false,
    images: [],
    categories: [
      {
        id: 5,
        name: "Books & Games",
        description: "Beatae incidunt saepe unde laboriosam culpa magnam.",
      },
    ],
    colors: [],
  },
  {
    id: 4,
    name: "Tasty Cotton Pants",
    price: 151.63,
    description:
      "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support",
    brand: "Treutel - Lindgren",
    stockQuantity: 73,
    averageRating: 0,
    isWished: false,
    sold: 16,
    isNew: false,
    images: [],
    categories: [
      {
        id: 3,
        name: "Home & Health",
        description:
          "Fugiat exercitationem corrupti asperiores voluptas odio aut ratione et occaecati.",
      },
      {
        id: 5,
        name: "Books & Games",
        description: "Beatae incidunt saepe unde laboriosam culpa magnam.",
      },
    ],
    colors: [],
  },
  {
    id: 5,
    name: "Awesome Plastic Ball",
    price: 677.07,
    description:
      "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients",
    brand: "Hilll and Sons",
    stockQuantity: 22,
    averageRating: 0,
    isWished: false,
    sold: 17,
    isNew: false,
    images: [],
    categories: [
      {
        id: 2,
        name: "Games, Sports & Beauty",
        description: "Animi distinctio non cum quia quos modi non laborum est.",
      },
      {
        id: 3,
        name: "Home & Health",
        description:
          "Fugiat exercitationem corrupti asperiores voluptas odio aut ratione et occaecati.",
      },
    ],
    colors: [],
  },
  {
    id: 6,
    name: "Small Concrete Fish",
    price: 765.09,
    description:
      "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit",
    brand: "Schmidt - Rolfson",
    stockQuantity: 16,
    averageRating: 0,
    isWished: false,
    sold: 48,
    isNew: false,
    images: [],
    categories: [
      {
        id: 3,
        name: "Home & Health",
        description:
          "Fugiat exercitationem corrupti asperiores voluptas odio aut ratione et occaecati.",
      },
      {
        id: 4,
        name: "Baby",
        description:
          "Culpa omnis illo a quia assumenda vel deserunt eos veniam minima dolorem aspernatur.",
      },
      {
        id: 5,
        name: "Books & Games",
        description: "Beatae incidunt saepe unde laboriosam culpa magnam.",
      },
    ],
    colors: [],
  },
];

export const mockCartList = [];

export const fetchDetailProduct = (productId) => {
  if (!productId) return;
  return mockProductList.find((product) => product.id == productId) || null;
};
