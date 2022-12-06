#include <iostream>
#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

GLuint Program;
GLuint VAO, VBO_position, VBO_color, EBO, Attrib_vertex, Attrib_color, Unif_model;
glm::mat4 model, scale, rotate, translate;

const char* VertexShaderSource = R"(
    #version 330 core
    in vec3 coord;
    in vec4 color;

    out vec4 vert_color;

    uniform mat4 model;

    void main() {
        gl_Position = model * vec4(coord, 1.0);

        vert_color = color;
    }
)";

const char* FragmentShaderSource = R"(
    #version 330 core
    in vec4 vert_color;

    void main() {
        gl_FragColor  = vert_color;
    }
)";

void Init();
void Draw();

int main() {
    sf::Window window(sf::VideoMode(600, 600), "Task 12.1", sf::Style::Default, sf::ContextSettings(24));
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
                case (sf::Keyboard::Left): translate = glm::translate(translate, glm::vec3(-0.1, 0.0, 0.0)); break;
                case (sf::Keyboard::Right): translate = glm::translate(translate, glm::vec3(0.1, 0.0, 0.0)); break;
                case (sf::Keyboard::Up): translate = glm::translate(translate, glm::vec3(0.0, 0.1, 0.0)); break;
                case (sf::Keyboard::Down): translate = glm::translate(translate, glm::vec3(0.0, -0.1, 0.0)); break;
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
    glDeleteBuffers(1, &EBO);

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
    float x1 = (float)glm::sqrt(1.2 * 1.2 - 0.6 * 0.6);
    float x2 = (float)glm::sqrt(x1 * x1 - x1 * x1 / 9);

    glm::vec3 v0 = { -0.6, 0.0, 0.0 };
    glm::vec3 v1 = { 0.0, float(x1), 0.0 };
    glm::vec3 v2 = { 0.6, 0.0, 0.0 };
    glm::vec3 v3 = { 0.0, float(x1 / 3), float(-x2) };

    glm::vec3 vertexes[] = { v0, v1, v2, v3 };

    GLuint indices[] = {
       2, 3, 0,
       1, 3, 0,
       2, 1, 0,
       2, 3, 1
    };

    glm::vec4 colors[] = {
        glm::vec4(0.0, 0.0, 1.0, 1.0),
        glm::vec4(1.0, 0.0, 0.0, 1.0),
        glm::vec4(0.0, 1.0, 0.0, 1.0),
        glm::vec4(1.0, 1.0, 1.0, 1.0)
    };

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO_position);
    glGenBuffers(1, &VBO_color);
    glGenBuffers(1, &EBO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertexes), vertexes, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_vertex);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_color);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);

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

    Unif_model = glGetUniformLocation(Program, "model");
    if (Unif_model == -1)
    {
        std::cout << "could not bind unif model" << std::endl;
        return;
    }

    glDeleteShader(vShader);
    glDeleteShader(fShader);

    checkOpenGLerror();
}

void InitMatrix()
{
    scale = glm::mat4(1.0f);
    rotate = glm::rotate(glm::mat4(1.0f), glm::radians(-10.0f), glm::vec3(0.0, 1.0, 0.0));
    translate = glm::translate(glm::mat4(1.0f), glm::vec3(0.0, -0.4, 0.0));
}

void Init() {
    InitShader();
    InitVAO();
    InitMatrix();
    glEnable(GL_DEPTH_TEST);
}

void Draw() {
    glUseProgram(Program);
    glBindVertexArray(VAO);

    model = scale * translate * rotate;

    glUniformMatrix4fv(Unif_model, 1, GL_FALSE, glm::value_ptr(model));
    glDrawElements(GL_TRIANGLES, 12, GL_UNSIGNED_INT, 0);

    glBindVertexArray(0);
    glUseProgram(0);

    checkOpenGLerror();
}