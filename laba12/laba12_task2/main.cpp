#include <iostream>
#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

GLuint Program;
GLuint VAO, VBO_position, VBO_color, VBO_text_coord, EBO, texture, Attrib_vertex, Attrib_color, Attrib_texture, Unif_model, Unif_mix_ratio;
glm::mat4 model, scale, rotate, translate;
sf::Image texture_image;
float mix_ratio = 0.5;

const char* VertexShaderSource = R"(
    #version 330 core
    in vec2 text_coord;
    in vec3 coord;
    in vec4 color;

    out vec4 vert_color;
    out vec2 vert_text_coord;

    uniform mat4 model;

    void main() {
        gl_Position = model * vec4(coord, 1.0);

        vert_color = color;
        vert_text_coord = text_coord;
    }
)";

const char* FragmentShaderSource = R"(
    #version 330 core
    in vec4 vert_color;
    in vec2 vert_text_coord;

    uniform sampler2D frag_text;
    uniform float ratio;

    void main() {
        vec4 text = texture(frag_text, vert_text_coord);
        gl_FragColor =  mix(text, vert_color, ratio);
    }
)";

void Init();
void Draw();

int main() {
    sf::Window window(sf::VideoMode(600, 600), "Task 12.2", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);

    window.setActive(true);

    glewInit();

    Init();

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }
            else if (event.type == sf::Event::Resized) {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case (sf::Keyboard::Up): mix_ratio = glm::min(mix_ratio + 0.1, 1.0); break;
                case (sf::Keyboard::Down): mix_ratio = glm::max(mix_ratio - 0.1, 0.0); break;
                default: break;
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();

        window.display();
    }

    glDeleteProgram(Program);
    glDeleteBuffers(1, &VBO_position);
    glDeleteBuffers(1, &VBO_color);
    glDeleteBuffers(1, &VBO_text_coord);
    glDeleteBuffers(1, &EBO);
    glDeleteBuffers(1, &VAO);

    return 0;
}

void checkOpenGLerror() {
    GLenum errCode;
    // https://www.khronos.org/opengl/wiki/OpenGL_Error
    if ((errCode = glGetError()) != GL_NO_ERROR)
        std::cout << "OpenGl error!: " << errCode << std::endl;
}

void ShaderLog(unsigned int shader) {
    int infologLen = 0;
    int charsWritten = 0;
    char* infoLog;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        infoLog = new char[infologLen];
        if (infoLog == NULL)
        {
            std::cout << "ERROR: Could not allocate InfoLog buffer" << std::endl;
            exit(1);
        }
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
        std::cout << "InfoLog: " << infoLog << "\n\n\n";
        delete[] infoLog;
    }
}


void InitVAO() {

    // �������� �����
    glm::vec3 v0 = { -0.5, -0.5, -0.5 };  // ����� ������ �������
    glm::vec3 v1 = { 0.5, -0.5, -0.5 };   // ������ ������ �������
    glm::vec3 v2 = { 0.5, 0.5, -0.5 };    // ������ ������� �������
    glm::vec3 v3 = { -0.5, 0.5, -0.5 };   // ����� ������� �������
    // ������ �����
    glm::vec3 v4 = { 0.5, -0.5, 0.5 };
    glm::vec3 v5 = { -0.5, -0.5, 0.5 };
    glm::vec3 v7 = { 0.5, 0.5, 0.5 };
    glm::vec3 v6 = { -0.5, 0.5, 0.5 };  
    // ������� �����
    glm::vec3 v8 = v3;
    glm::vec3 v9 = v2;
    glm::vec3 v10 = v7;
    glm::vec3 v11 = v6;
    // ������ �����
    glm::vec3 v12 = v5;
    glm::vec3 v13 = v4;
    glm::vec3 v14 = v1;
    glm::vec3 v15 = v0;
    // ����� �����
    glm::vec3 v16 = v5;
    glm::vec3 v17 = v0;
    glm::vec3 v18 = v3;
    glm::vec3 v19 = v6;
    // ������ �����
    glm::vec3 v20 = v1;
    glm::vec3 v21 = v4;
    glm::vec3 v22 = v7;
    glm::vec3 v23 = v2;


    glm::vec3 vertexes[] = { v0, v1, v2, v3, 
                             v4, v5, v6, v7,
                             v8, v9, v10, v11,
                             v12, v13, v14, v15,
                             v16, v17, v18, v19,
                             v20, v21, v22, v23 
    };

    GLuint indices[] = {
                         0, 1, 2,
                         0, 2, 3,

                         4, 5, 6,
                         4, 6, 7,
                         
                         8, 9, 10,
                         8, 10, 11,

                         12, 13, 14,
                         12, 14, 15,
        
                         16, 17, 18,
                         16, 18, 19,

                         20, 21, 22,
                         20, 22, 23
    };

    glm::vec4 c_0 = { 1.0, 0.0, 0.0, 1.0 }; // red
    glm::vec4 c_1 = { 1.0, 1.0, 1.0, 1.0 }; // white
    glm::vec4 c_2 = { 0.0, 1.0, 0.0, 1.0 }; // green
    glm::vec4 c_3 = { 1.0, 1.0, 0.0, 1.0 }; // yellow

    glm::vec4 c_4 = { 1.0, 0.0, 1.0, 1.0 }; // purple
    glm::vec4 c_5 = { 0.0, 1.0, 1.0, 1.0 }; // cyan
    glm::vec4 c_6 = { 0.0, 0.0, 1.0, 1.0 }; // blue
    glm::vec4 c_7 = { 0.0, 0.0, 0.0, 1.0 }; // black

    glm::vec4 c_8 = c_3;
    glm::vec4 c_9 = c_2;
    glm::vec4 c_10 = c_7;
    glm::vec4 c_11 = c_6;

    glm::vec4 c_12 = c_5;
    glm::vec4 c_13 = c_4;
    glm::vec4 c_14 = c_1;
    glm::vec4 c_15 = c_0;

    glm::vec4 c_16 = c_5;
    glm::vec4 c_17 = c_0;
    glm::vec4 c_18 = c_3;
    glm::vec4 c_19 = c_6;

    glm::vec4 c_20 = c_1;
    glm::vec4 c_21 = c_4;
    glm::vec4 c_22 = c_7;
    glm::vec4 c_23 = c_2;


    glm::vec4 colors[] = { c_0, c_1, c_2, c_3,
                           c_4, c_5, c_6, c_7,
                           c_8, c_9, c_10, c_11,
                           c_12, c_13, c_14, c_15,
                           c_16, c_17, c_18, c_19,
                           c_20, c_21, c_22, c_23 
    };

    glm::vec2 tc_0 = { 0.0, 0.0 };
    glm::vec2 tc_1 = { 1.0, 0.0 };
    glm::vec2 tc_2 = { 1.0, 1.0 };
    glm::vec2 tc_3 = { 0.0, 1.0 };

    glm::vec2 tc_4 = tc_0;
    glm::vec2 tc_5 = tc_1;
    glm::vec2 tc_6 = tc_2;
    glm::vec2 tc_7 = tc_3;

    glm::vec2 tc_8 = tc_0;
    glm::vec2 tc_9 = tc_1;
    glm::vec2 tc_10 = tc_2;
    glm::vec2 tc_11 = tc_3;

    glm::vec2 tc_12 = tc_0;
    glm::vec2 tc_13 = tc_1;
    glm::vec2 tc_14 = tc_2;
    glm::vec2 tc_15 = tc_3;

    glm::vec2 tc_16 = tc_0;
    glm::vec2 tc_17 = tc_1;
    glm::vec2 tc_18 = tc_2;
    glm::vec2 tc_19 = tc_3;

    glm::vec2 tc_20 = tc_0;
    glm::vec2 tc_21 = tc_1;
    glm::vec2 tc_22 = tc_2;
    glm::vec2 tc_23 = tc_3;
   

    glm::vec2 text_coord[] = { tc_0, tc_1, tc_2, tc_3, 
                               tc_4, tc_5, tc_6, tc_7,
                               tc_8, tc_9, tc_10, tc_11,
                               tc_12, tc_13, tc_14, tc_15,
                               tc_16, tc_17, tc_18, tc_19,
                               tc_20, tc_21, tc_22, tc_23 };

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO_position);
    glGenBuffers(1, &VBO_color);
    glGenBuffers(1, &VBO_text_coord);
    glGenBuffers(1, &EBO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_text_coord);
    glBufferData(GL_ARRAY_BUFFER, sizeof(text_coord), text_coord, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertexes), vertexes, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_vertex);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_color);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_text_coord);
    glVertexAttribPointer(Attrib_texture, 2, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_texture);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);
    glDisableVertexAttribArray(Attrib_texture);

    checkOpenGLerror();
}


void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    std::cout << "vertex shader \n";
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &FragmentShaderSource, NULL);
    glCompileShader(fShader);
    std::cout << "fragment shader \n";
    ShaderLog(fShader);

    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    glLinkProgram(Program);

    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
        return;
    }

    Attrib_vertex = glGetAttribLocation(Program, "coord");
    if (Attrib_vertex == -1)
    {
        std::cout << "could not bind attrib coord" << std::endl;
        return;
    }

    Attrib_color = glGetAttribLocation(Program, "color");
    if (Attrib_color == -1)
    {
        std::cout << "could not bind attrib color" << std::endl;
        return;
    }

    Attrib_texture = glGetAttribLocation(Program, "text_coord");
    if (Attrib_texture == -1)
    {
        std::cout << "could not bind attrib texture" << std::endl;
        return;
    }

    Unif_model = glGetUniformLocation(Program, "model");
    if (Unif_model == -1)
    {
        std::cout << "could not bind unif model" << std::endl;
        return;
    }

    Unif_mix_ratio = glGetUniformLocation(Program, "ratio");
    if (Unif_mix_ratio == -1)
    {
        std::cout << "could not bind unif mix ratio" << std::endl;
        return;
    }

    glDeleteShader(vShader);
    glDeleteShader(fShader);

    checkOpenGLerror();
}

void InitMatrix() {
    scale = glm::mat4(1.0f);
    rotate = glm::mat4(1.0f);
    rotate = glm::rotate(glm::mat4(1.0f), glm::radians(-135.0f), glm::vec3(0.0, 1.0, 0.0));
    rotate = glm::rotate(rotate, glm::radians(-30.0f), glm::vec3(1.0, 0.0, 0.0));
    translate = glm::mat4(1.0f);
}

void InitTexture() {
    if (!texture_image.loadFromFile("texture.jpg")) 
    {
        std::cout << "could not load texture" << std::endl;
        return;
    }
    texture_image.flipVertically();
    glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, texture_image.getSize().x, texture_image.getSize().y, 0, GL_RGBA, GL_UNSIGNED_BYTE, texture_image.getPixelsPtr());
    glGenerateMipmap(GL_TEXTURE_2D);
    glBindTexture(GL_TEXTURE_2D, 0);
}

void Init() {
    InitShader();
    InitVAO();
    InitMatrix();
    InitTexture();

    glEnable(GL_DEPTH_TEST);
    glEnable(GL_TEXTURE_2D);
}

void Draw() {
    glUseProgram(Program);
    glBindTexture(GL_TEXTURE_2D, texture);
    glBindVertexArray(VAO);

    model = scale * translate * rotate;

    glUniformMatrix4fv(Unif_model, 1, GL_FALSE, glm::value_ptr(model));
    glUniform1f(Unif_mix_ratio, mix_ratio);
    glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);

    glBindVertexArray(0);
    glBindTexture(GL_TEXTURE_2D, 0);
    glUseProgram(0);

    checkOpenGLerror();
}